using Microsoft.AspNetCore.Mvc;
using Core.Negocio.Predios;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrediosController : ControllerBase
    {
        // GET: api/<PrediosController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Models.Predio> predios = new List<Models.Predio>();
                var prediosGetResponse = Predio.ObtenerTodos();
                if (prediosGetResponse.Resultado != Core.Tuneles.StatusCode.OK || prediosGetResponse.Contenido == null)
                {
                    throw new InternalServerException(prediosGetResponse.Mensaje);
                }

                foreach (var predio in prediosGetResponse.Contenido)
                {
                    predios.Add(new Models.Predio()
                    {
                        id = predio.Id,
                        direccion = predio.Direccion,
                        email = predio.Mail,
                        nombre = predio.Nombre,
                        localidad = (int)predio.Localidad,
                        fotoPerfil = predio.FotoPerfil,
                        fotoPortada = predio.FotoPortada,
                        habilitado = predio.Habilitado,
                        idUsuarioPredio = predio.Usuario?.Id,
                        horario = predio.Horarios.Select(h => new Models.Horario(h.Predio.Id.Value, h.Hora, (int)h.DiaSemana)).ToList()
                    });
                }

                return Ok(new ResponseTemplate(EResponseConcept.PREDIO_GET_ALL, Core.Tuneles.StatusCode.OK, null, null, predios, null));

            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_GET_ALL, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_GET_ALL, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }

        // GET api/<PrediosController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var predio_res = new Models.Predio();
                var predioGetResponse = Predio.Obtener(id);
                if (predioGetResponse.Resultado != Core.Tuneles.StatusCode.OK || predioGetResponse.Contenido == null)
                {
                    throw new InternalServerException(predioGetResponse.Mensaje);
                }
                var pr = predioGetResponse.Contenido;
                predio_res = new Models.Predio()
                {
                    descripcion = pr.Descripcion,
                    email = pr.Mail,
                    direccion = pr.Direccion,
                    preferencias = pr?.Preferencias?.Select(p => (int)p).ToList(),
                    horario = pr.Horarios?.Select(h => new Models.Horario(h.Predio?.Id, h.Hora, h.DiaSemana.HasValue ? (int)h.DiaSemana.Value : null)).ToList(),
                    fotoPerfil = pr.FotoPerfil,
                    fotoPortada = pr.FotoPortada,
                    id = pr.Id,
                    habilitado = pr.Habilitado,
                    localidad = (int)pr.Localidad,
                    nombre = pr.Nombre
                };
                return Ok(new ResponseTemplate(EResponseConcept.PREDIO_GET, Core.Tuneles.StatusCode.OK, null, null, predio_res, null));

            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_GET, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_GET, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }

        // POST api/<PrediosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PrediosController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Models.Predio req)
        {
            try
            {
                #region Chequeo nulls
                if (req.localidad == null) throw new BadRequestException("No se ingresó la localidad");
                if (req.nombre == null) throw new BadRequestException("No se ingresó el nombre");
                if (req.habilitado == null) throw new BadRequestException("No se ingresó el estado de habilitación del predio");
                if (req.horario == null) throw new BadRequestException("No se ingresaron los horarios del predio");
                if (req.horario.Any(h => h.hora == null || h.idPredio == null || h.diaSemana == null || !Enum.IsDefined(typeof(DayOfWeek), h.diaSemana))) throw new BadRequestException("La/s horas introducidass no tiene/n el formato correcto");
                if (req.direccion == null) throw new BadRequestException("No se ha introducido la dirección");
                if (req.idUsuarioPredio == null) throw new BadRequestException("No se ha introducido el usuario asociado al predio");
                
                #endregion

                #region Validaciones
                if (req.preferencias != null && req.preferencias.Any(p => !Enum.IsDefined(typeof(Core.Negocio.Preferencias.Preferencia), p))) throw new BadRequestException("Una de las preferencias elegidas no existen");
                if (!Enum.IsDefined(typeof(Core.Negocio.Juridicciones.Localidad), req.localidad)) throw new BadRequestException("La localidad introducida no es valida");
                #endregion
                var predioResponse = Predio.Obtener(id);

                if (predioResponse.Resultado != Core.Tuneles.StatusCode.OK)
                    throw new InternalServerException(predioResponse.Mensaje);
                if (predioResponse.Contenido == null)
                    throw new NotFoundException("No se pudo encontrar el predio solicitado");

                Predio predio = predioResponse.Contenido;

                predio.Id = id;
                predio.Descripcion = req.descripcion;
                predio.FotoPerfil = req.fotoPerfil;
                predio.FotoPortada = req.fotoPortada;
                predio.Horarios = req.horario.Select(h => new Core.Negocio.HorariosPredios.HorarioPredio()
                {
                    DiaSemana = (DayOfWeek)h.diaSemana.Value,
                    Predio = predio,
                    Hora = h.hora
                }).ToList();
                predio.Mail = req.email;
                predio.Preferencias = req.preferencias == null ? null : req.preferencias.Select(p => (Core.Negocio.Preferencias.Preferencia)p).ToList();
                predio.Localidad = (Core.Negocio.Juridicciones.Localidad)req.localidad;
                predio.Habilitado = req.habilitado.Value;


                predio.Modificar();
                return Ok(new ResponseTemplate(EResponseConcept.PREDIO_UPDATE, Core.Tuneles.StatusCode.OK, null, null, null, null));


            }
            catch (BadRequestException bde)
            {
                return BadRequest(new ResponseTemplate(EResponseConcept.PREDIO_UPDATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.BAD_REQUEST,bde.Msg, null, null));

            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_UPDATE, Core.Tuneles.StatusCode.OK, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PREDIO_UPDATE, Core.Tuneles.StatusCode.OK, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }

        // DELETE api/<PrediosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
