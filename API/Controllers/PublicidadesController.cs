using Microsoft.AspNetCore.Mvc;
using Core.Negocio.Publicidades;
using System.Globalization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PublicidadesController : ControllerBase
    {
        // GET: api/<PublicidadesController>
        [HttpGet]
        public async Task<ResponseTemplate> Get()
        {
            PublicidadListaResponse publicidadData = Publicidad.ObtenerTodas();
            return new ResponseTemplate(EResponseConcept.PUBLICIDAD_GET_ALL, publicidadData.Resultado, null, publicidadData.Mensaje, publicidadData.Contenido, null);
        }

        // GET api/<PublicidadesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PublicidadesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Publicidad? req)
        {
            try
            {
                #region Chequeo nulls
                if (req.nombre == null) throw new BadRequestException("No se ha dado nombre a la publicidad") { Source = "BAD_REQUEST" };
                if (req.resolucion == null) throw new BadRequestException("No se ha dado una resolución a la imagen") { Source = "BAD_REQUEST" };
                if (req.archivo == null) throw new BadRequestException("No se ha dado un nombre de archivo a la publicidad") { Source = "BAD_REQUEST" };
                if (req.tipo == null) throw new BadRequestException("No se ha dado seleccionado el tipo de publicidad") { Source = "BAD_REQUEST" };

                #endregion

                #region Chequeo formato fecha
                DateTime? fechaExpDt = null;

                try
                {
                    if (req.fechaExpiracion != null)
                        fechaExpDt = DateTime.ParseExact(req.fechaExpiracion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (FormatException exFecha)
                {
                    throw new FormatException("La fecha introducida no tiene el formato correcto") { Source = "BAD_REQUEST" };
                }
                #endregion

                #region Chequeo Tipo publicidad
                Core.Negocio.TiposPublicidad? tipoPub;
                try
                {
                    if (!Enum.IsDefined(typeof(Core.Negocio.TiposPublicidad), req.tipo))
                    {
                        throw new BadRequestException("El tipo de publicidad introducido no es válido");
                    }
                    tipoPub = (Core.Negocio.TiposPublicidad)req.tipo;
                }
                catch (Exception ex)
                {
                    throw new Exception("El tipo de publicidad introducido no es válido");
                }

                #endregion

                Publicidad publicidad = new();

                publicidad.Nombre = req.nombre;
                publicidad.Resolucion = req.resolucion;
                publicidad.Archivo = req.archivo;
                publicidad.FechaExpiracion = fechaExpDt;
                publicidad.Tipo = tipoPub;
                var pubResponse = publicidad.Crear();
                if (pubResponse.Resultado == Core.Tuneles.StatusCode.OK)
                {
                    return Ok(new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.OK, null, pubResponse.Mensaje, pubResponse.Contenido.Id, null));
                }
                else
                {
                    throw new InternalServerException(pubResponse.Mensaje);
                }
            }
            catch (BadRequestException bre)
            {
                return BadRequest(new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.BAD_REQUEST, bre.Msg, null, null));
            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }

        // PUT api/<PublicidadesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int? id, [FromBody] Models.Publicidad req)
        {
            try
            {
                #region Chequeo nulls
                if (id == null) throw new Exception("No se ha dado ningún id de publicidad");
                if (req.nombre == null) throw new BadRequestException("No se ha dado nombre a la publicidad") { Source = "BAD_REQUEST" };
                if (req.resolucion == null) throw new BadRequestException("No se ha dado una resolución a la imagen") { Source = "BAD_REQUEST" };
                if (req.archivo == null) throw new BadRequestException("No se ha dado un nombre de archivo a la publicidad") { Source = "BAD_REQUEST" };
                if (req.tipo == null) throw new BadRequestException("No se ha dado seleccionado el tipo de publicidad") { Source = "BAD_REQUEST" };

                #endregion

                #region Chequeo formato fecha
                DateTime? fechaExpDt = null;

                try
                {
                    if (req.fechaExpiracion != null)
                        fechaExpDt = DateTime.ParseExact(req.fechaExpiracion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (FormatException exFecha)
                {
                    throw new FormatException("La fecha introducida no tiene el formato correcto") { Source = "BAD_REQUEST" };
                }
                #endregion

                #region Chequeo Tipo publicidad
                Core.Negocio.TiposPublicidad? tipoPub;
                try
                {
                    if (!Enum.IsDefined(typeof(Core.Negocio.TiposPublicidad), req.tipo))
                    {
                        throw new BadRequestException("El tipo de publicidad introducido no es válido");
                    }
                    tipoPub = (Core.Negocio.TiposPublicidad)req.tipo;
                }
                catch (Exception ex)
                {
                    throw new Exception("El tipo de publicidad introducido no es válido");
                }

                #endregion

                var publicidadGetResponse = Publicidad.Obtener(id.Value);

                if(publicidadGetResponse.Resultado != Core.Tuneles.StatusCode.OK || publicidadGetResponse.Contenido == null)
                {
                    throw new InternalServerException(publicidadGetResponse.Mensaje);
                }

                Publicidad publicidad = publicidadGetResponse.Contenido;

                publicidad.Nombre = req.nombre;
                publicidad.Resolucion = req.resolucion;
                publicidad.Archivo = req.archivo;
                publicidad.FechaExpiracion = fechaExpDt;
                publicidad.Tipo = tipoPub;
                var pubResponse = publicidad.Modificar();
                if (pubResponse.Resultado == Core.Tuneles.StatusCode.OK)
                {
                    return Ok(new ResponseTemplate(EResponseConcept.PUBLICIDAD_UPDATE, Core.Tuneles.StatusCode.OK, null, pubResponse.Mensaje, pubResponse.Contenido.Id, null));
                }
                else
                {
                    throw new InternalServerException(pubResponse.Mensaje);
                }
            }
            catch (BadRequestException bre)
            {
                return BadRequest(new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.BAD_REQUEST, bre.Msg, null, null));
            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }

        // DELETE api/<PublicidadesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                #region Chequeo nulls
                if (id == null) throw new Exception("No se ha dado ningún id de publicidad");
                #endregion


                var publicidadGetResponse = Publicidad.Obtener(id.Value);

                if (publicidadGetResponse.Resultado != Core.Tuneles.StatusCode.OK || publicidadGetResponse.Contenido == null)
                {
                    throw new InternalServerException(publicidadGetResponse.Mensaje);
                }

                Publicidad publicidad = publicidadGetResponse.Contenido;

                var pubResponse = publicidad.Eliminar();
                if (pubResponse.Resultado == Core.Tuneles.StatusCode.OK)
                {
                    return Ok(new ResponseTemplate(EResponseConcept.PUBLICIDAD_UPDATE, Core.Tuneles.StatusCode.OK, null, pubResponse.Mensaje, pubResponse.Contenido.Id, null));
                }
                else
                {
                    throw new InternalServerException(pubResponse.Mensaje);
                }
            }
            catch (BadRequestException bre)
            {
                return BadRequest(new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.BAD_REQUEST, bre.Msg, null, null));
            }
            catch (InternalServerException ise)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ise.Msg, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseTemplate(EResponseConcept.PUBLICIDAD_CREATE, Core.Tuneles.StatusCode.ERROR, EErrorCode.INTERNAL_ERROR, ex.Message, null, null));
            }
        }
    }
}
