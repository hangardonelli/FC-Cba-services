#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Negocio.Canchas;
using Core.Negocio.Usuarios;
using Core.Negocio.Juridicciones;
using Core.Negocio.HorariosPredios;
using Core.Negocio.Preferencias;
using Core.Entorno;

namespace Core.Negocio.Predios
{
    /// <summary>
    /// Representa a un predio de FC Cordoba
    /// </summary>
    public class Predio
    {
        #region Propiedades públicas
        /// <summary>
        /// El identificador del predio
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// El nombre que se mostrará publicamente
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// El correo electrónico del predio
        /// </summary>
        public string? Mail { get; set; }

        /// <summary>
        /// Las canchas del predio
        /// </summary>

        /// <summary>
        /// Cuenta del panel de administración del predio
        /// </summary>
        public UsuarioPredio? Usuario { get; set; }

        /// <summary>
        /// Indica si el predio aparece habilitado (si se encuentra disponible en la app)
        /// </summary>
        public bool Habilitado { get; set; }

        /// <summary>
        /// La localidad del predio
        /// </summary>
        public Localidad Localidad { get; set; }

        /// <summary>
        /// La provincia del predio
        /// </summary>
        public Provincias Provincia { get; set; }

        public string? Direccion { get; set; }

        /// <summary>
        /// La foto de perfil del predio
        /// </summary>
        public string? FotoPerfil
        {
            get => Urls.FotoPerfil + _FotoPerfil;
            set => _FotoPerfil = value;
        }
        /// <summary>
        /// La foto de portada del predio
        /// </summary>
        public string? FotoPortada
        {
            get => Entorno.Urls.FotoPortada + _FotoPortada;
            set => _FotoPortada = value;
        }
        /// <summary>
        /// Son los horarios de apertura de un predio
        /// </summary>
        public List<HorarioPredio>? Horarios { get; set; }

        /// <summary>
        /// Son las preferencias del predio (parrilla, buffet, vestuario, etc)
        /// </summary>
        public List<Preferencia>? Preferencias { get; set; }

        /// <summary>
        /// La descripción o mensaje que va a presentar al predio en la app
        /// </summary>
        public string? Descripcion { get; set; }
        #endregion

        #region Propiedades privadas
        private string? _FotoPerfil { get; set; }
        private string? _FotoPortada { get; set; }

        #endregion

        #region Metodos públicos

        /// <summary>
        /// Crea la instancia actual del predio en la base de datos
        /// </summary>
        public PredioResponse Crear()
        {
            PredioResponse response = new() { AccionRealizada = AccionRealizada.CREAR };

            try
            {
                #region Validaciones de clase pre-inserción
                if (Nombre == null) throw new NullReferenceException("No se ha establecido un nombre para el predio");
                if (Usuario == null || Usuario.Id == null) throw new NullReferenceException("No se ha establecido un usuario para el predio");
                if (Mail == null) throw new NullReferenceException("No se ha establecido un correo electrónico para el predio");

                if (Nombre.Length > 20) throw new Exception("El nombre para el predio es demasiado largo");
                if (Nombre.Length < 4) throw new Exception("El nombre para el predio es demasiado corto");

                string mensajeMail = String.Empty;
                if (!Mail.ValidarEmail(out string? mensaje))
                    throw new Exception(mensaje);

                #endregion

                var crearPredio = Datos.Predio.Predio.Crear(Nombre, Mail, Usuario.Id.Value, _FotoPerfil, _FotoPortada, (int)Localidad, Direccion);


                #region Validaciones post-inserción
                if (crearPredio == null) throw new Exception("Hubo un error al crear el predio");
                if (crearPredio.Rows[0] == null || crearPredio.Rows[0]["id"] == null) throw new Exception("No se pudo obtener la identificación del predio");
                #endregion

                Id = Convert.ToInt32(crearPredio.Rows[0]["id"]);

                if (Horarios != null)
                {
                    foreach (HorarioPredio horario in Horarios)
                    {
                        var responseHorario = horario.Modificar();
                        if (responseHorario.Resultado != Tuneles.StatusCode.OK)
                            throw new Exception(responseHorario.Mensaje ?? "Se ha producido un error desconocido al cargar los horarios del predio");
                    }
                }
                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public static PredioListaResponse ObtenerTodos()
        {
            PredioListaResponse response = new PredioListaResponse() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                List<Predio> predios = new List<Predio>();
                DataTable datos = Datos.Predio.Predio.ObtenerTodos();
                foreach(DataRow row in datos.Rows)
                {
                    var predioResponse = Predio.Obtener(row);
                    if (predioResponse.Resultado == Tuneles.StatusCode.OK && predioResponse.Contenido != null)
                        predios.Add(predioResponse.Contenido);
                    else throw new Exception(predioResponse.Mensaje);
                }


                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = predios;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Obtiene un predio a partir de un DataRow
        /// </summary>
        public static PredioResponse Obtener(DataRow row)
        {
            PredioResponse response = new PredioResponse() { AccionRealizada = Predios.AccionRealizada.CONSULTAR };

            try
            {
                Predio predio = new();

                #region Chequeo nulls
                if (row["id"] == null) throw new Exception("No se pudo cargar el id del predio");
                if (row["nombre"] == null) throw new Exception("No se pudo cargar el nombre del predio");
                if (row["email"] == null) throw new Exception("No se pudo cargar el email del predio");
                if (row["perfil"] == null) throw new Exception("No se pudo cargar el perfil del predio");
                if (row["portada"] == null) throw new Exception("No se pudo cargar la portada del predio");
                if (row["localidad"] == null) throw new Exception("No se pudo cargar la localidad del predio");
                if (row["habilitado"] == null) throw new Exception("No se reconoce el estado de habilitación del predio");
                if (row["usuario"] == null) throw new Exception("No se reconoce el usuario del predio");
                if (row["descripcion"] == null) throw new Exception("No se reconoce la descripción del predio");
                #endregion

                #region Chequeo juridicciones
                if (!Int32.TryParse(row["localidad"].ToString(), out int r_localidad))
                    throw new Exception("La localidad no es valida");

                if (!Int32.TryParse(row["usuario"].ToString(), out int r_usuario))
                    throw new Exception("No es un usuario valido");
   

                if (!Enum.IsDefined(typeof(Localidad), r_localidad))
                    throw new Exception("No se reconoce la localidad" + r_localidad);
              

                #endregion

                #region Asignacion de valores
                predio.Id = Convert.ToInt32(row["id"]);
                predio.Nombre = row["nombre"].ToString();
                predio.Mail = row["email"].ToString();
                predio.FotoPerfil = row["perfil"].ToString();
                predio.FotoPortada = row["portada"].ToString();
                predio.Localidad = (Localidad)r_localidad;
                predio.Descripcion = row["descripcion"].ToString();
                UsuarioPredioResponse obtencionUsuario = UsuarioPredio.Obtener(r_usuario);
                if (obtencionUsuario.Resultado == Tuneles.StatusCode.OK)
                    predio.Usuario = obtencionUsuario.Contenido;
                #endregion

                #region Cargar preferencias del predio
                var preferenciasResponse = Negocio.Preferencias.Preferencias.Obtener(predio);
                if (preferenciasResponse.Resultado != Tuneles.StatusCode.OK) throw new Exception(preferenciasResponse.Mensaje);
                predio.Preferencias = preferenciasResponse.Contenido;
                #endregion

                response.Contenido = predio;
                response.Adicional = predio.ToXML();
                response.Resultado = Tuneles.StatusCode.OK;

                #region Obtención Horarios
                var horariosResponse = HorarioPredio.Obtener(predio);
                if (horariosResponse.Resultado == Tuneles.StatusCode.OK && horariosResponse.Contenido != null)
                    predio.Horarios = horariosResponse.Contenido;
                else throw new Exception(horariosResponse.Mensaje);
                #endregion


            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Error al obtener el predio: " + ex.Message;
            }

            return response;
        }



        private List<CanchaFutbol> obtenerCanchasRec(CanchaFutbol cancha, List<CanchaFutbol> obtenidas)
        {
            foreach (CanchaFutbol c in cancha.Divisiones)
            {
                obtenidas.Add(c);
                obtenerCanchasRec(c, obtenidas);
            }

            return obtenidas;
        }
        /// <summary>
        /// Obtener las canchas de un predio obviando la estructura jerarquica
        /// </summary>
        /// 

        public PredioCanchaResponse ObtenerCanchasNoArbol()
        {
            PredioCanchaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                var canchasPredio = ObtenerCanchas().Contenido;
                List<CanchaFutbol> canchasADevolver = new List<CanchaFutbol>();
                canchasADevolver.AddRange(canchasPredio);

                foreach (CanchaFutbol cancha in canchasPredio)
                    canchasADevolver.AddRange(obtenerCanchasRec(cancha, canchasADevolver));

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = new();

                foreach(CanchaFutbol cancha in canchasADevolver)
                {
                    if(!response.Contenido.Exists(c => c.Id == cancha.Id && c.Predio.Id == cancha.Predio.Id))
                    {
                        response.Contenido.Add(cancha);
                    }
                }
                
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Error al obtener la lista de canchas: " + ex.Message;

            }

            return response;

        }
        /// <summary>
        /// Obtiene la lista de canchas de un predio;
        /// </summary>
        public PredioCanchaResponse ObtenerCanchas()
        {
            PredioCanchaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTARCANCHA };
            List<CanchaFutbol> canchas = new();
            try
            {
                if (Id == null) throw new NullReferenceException("No se ha establecido el id de la cancha");
                DataTable datos = Datos.Predio.Predio.ObtenerCanchas(Id.Value);

                foreach (DataRow row in datos.Rows)
                {
                    if (row["id"] == null) throw new Exception("No se pudo obtener la identificación de la cancha");
                    CanchaResponse respuestaCancha = CanchaFutbol.Obtener(row["id"].ToString(), this);
                    if (respuestaCancha == null || respuestaCancha.Contenido == null) throw new Exception("No se pudo obtener la cancha");
                    if (respuestaCancha.Resultado == Tuneles.StatusCode.OK)
                        canchas.Add(respuestaCancha.Contenido);
                    else
                        throw new Exception("Error al obtener una de las canchas. " + respuestaCancha.Mensaje);

                }
                response.Contenido = canchas;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.CodigoError = "OBTENERCANCHAS_ERR";
                response.Mensaje = ex.Message;
            }

            response.Adicional = canchas.ToXML();
            return response;
        }

        /// <summary>
        /// Obtiene un predio por su ID
        /// </summary>
        public static PredioResponse Obtener(int id)
        {
            PredioResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            Predio predio = new();
            try
            {
                DataTable datos = Datos.Predio.Predio.Obtener(id);

                if (datos.Rows.Count > 0)
                {
                    foreach (DataRow row in datos.Rows)
                    {
                        var predioResponse = Predio.Obtener(row);
                        if (predioResponse.Resultado == Tuneles.StatusCode.OK && predioResponse.Contenido != null)
                            predio = predioResponse.Contenido;
                        else throw new Exception(predioResponse.Mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }
            response.Contenido = predio;
            return response;
        }

        /// <summary>
        /// Guarda en base de datos las modificaciones realizadas a esta instancia
        /// </summary>
        public PredioResponse Modificar()
        {
            PredioResponse response = new() { AccionRealizada = AccionRealizada.MODIFICAR };

            try
            {
                if (Id == null) throw new Exception("No se ha establecido un identificador para este predio");
                if (Nombre == null) throw new Exception("No se ha establecido un nombre para este predio");
                if (Mail == null) throw new Exception("No se ha establecido una dirección de correo electrónico para este predio");
                if (Direccion == null) throw new Exception("No se ha establecido una dirección para este predio");

                if (Mail.ValidarEmail(out string? mensajeMail)) throw new Exception(mensajeMail);
                if (!Direccion.ValidarDireccion()) throw new Exception("La dirección del predio no es valida");


                Datos.Predio.Predio.Modificar(Id.Value, Nombre, Mail, FotoPerfil, FotoPortada, (int)Localidad, Direccion);

                response.Resultado = Tuneles.StatusCode.OK;
                response.Adicional = this;

                return response;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
                return response;
            }
        }
        #endregion
    }


}



