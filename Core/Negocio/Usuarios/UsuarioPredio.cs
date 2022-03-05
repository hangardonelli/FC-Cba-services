using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Core.Negocio.Usuarios
{
    /// <summary>
    /// Representa al usuario de los predios
    /// </summary>
    public class UsuarioPredio : UsuarioBase
    {
        #region Propiedades publicas
        public string? Latitud { get; set; }
        public string? Longitud { get; set; }
        public string? Telefono
        {
            get => _Telefono;
            set => _Telefono = value?.Trim();
        }
        #endregion

        #region Propiedades privadas
        protected string? _Telefono { get; set; }

        #endregion
        


        #region Metodos publicos
        /// <summary>
        /// Crea un usuario de predio
        /// </summary>
        public UsuarioPredioResponse Crear()
        {
            UsuarioPredioResponse response = new UsuarioPredioResponse() { AccionRealizada = AccionRealizada.CREAR };
            try
            {
                if(ValidarRegistro(out string? mensajeError))
                {
                    DataTable datos = Datos.Usuarios.UsuarioPredio.Crear(Email, _Password, _NroDocumento, Latitud, Longitud, _Telefono);
                    int id = Convert.ToInt32(datos.Rows[0]["id"]);

                    Id = id;
                    response.Contenido = this;
                    response.Resultado = Tuneles.StatusCode.OK;
                }
                else throw new Exception("Error durante la validación de datos: " + mensajeError);

            }
            catch (Exception ex)
            {
                response.Mensaje = ex.Message;
                response.Resultado = Tuneles.StatusCode.ERROR;
            }

            response.Adicional = this.ToXML();
            return response;

        }



        /// <summary>
        /// Obtiene un usuario de predio por su mail
        /// </summary>
        public static UsuarioPredioResponse Obtener(string mail)
        {
            UsuarioPredioResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                UsuarioPredio usuario = new UsuarioPredio();
                DataTable usuarioDatos = Datos.Usuarios.UsuarioPredio.Obtener(mail);

                if (usuarioDatos.Rows.Count == 0) throw new UserNotFoundException();
                if (usuarioDatos.Rows.Count > 1) throw new AmbiguousUserException();
                foreach (DataRow row in usuarioDatos.Rows)
                {
                    #region Chequeo Nulls
                    if (row["id"] == null) throw new Exception("No se ha cargado programáticamente el id para el usuario");
                    if (row["email"] == null) throw new Exception("No se ha cargado programáticamente el email para el usuario");
                    if (row["telefono"] == null) throw new Exception("No se ha cargado programáticamente el telefono para el usuario");
                    if (row["fecha_registro"] == null) throw new Exception("No se ha cargado programáticamente la fecha del registro para el usuario");
                    #endregion

                    #region Chequeo formato
                    if (!Int32.TryParse(row["id"].ToString(), out int r_id)) throw new FormatException("El id del usuario no tiene el formato correcto");
                    if (!Boolean.TryParse(row["habilitado"].ToString(), out bool r_habilitado)) throw new FormatException("La habilitación del usuario no tiene el formato correcto");
                    #endregion

                    usuario.Id = r_id;
                    usuario.NroDocumento = row["nroDocumento"].ToString();
                    usuario.Email = row["email"].ToString();
                    usuario.Password = row["password"].ToString();
                    usuario.FechaRegistro = Convert.ToDateTime(row["registro_fecha"]);
                    usuario.Longitud = row["coords_lg"].ToString();
                    usuario.Latitud = row["coords_lt"].ToString();
                }

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = usuario;

            }
            catch (AmbiguousUserException)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se encontró más de un usuario con el mismo correo electrónico. No fue posible obtenerlo, hay ambigüedad.";
            }
            catch (UserNotFoundException)
            {
                response.Resultado = Tuneles.StatusCode.WARNING;
                response.Mensaje = "No se ha encontrado al usuario solicitado";
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Verificar si la contraseña introducida coincide con la del usuario
        /// </summary>
        public UsuarioPredioResponse VerificarPassword(string passwordPlana)
        {
            UsuarioPredioResponse response = new() { AccionRealizada = AccionRealizada.VALIDARCREDENCIALES };

            try
            {
                if (BCrypt.Net.BCrypt.Verify(passwordPlana, _Password))
                {
                    response.Resultado = Tuneles.StatusCode.OK;
                    response.Contenido = this;
                    return response;
                }
                else throw new InvalidCredentialException();
            }
            catch (InvalidCredentialException)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Las credenciales no son correctas";
                response.CodigoError = "INVALID_CREDENTIALS_PREDIO";
                return response;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// Obtiene un usuario de un predio por su id
        /// </summary>
        public static UsuarioPredioResponse Obtener(int id)
        {
            UsuarioPredio? usuario = new UsuarioPredio();
            UsuarioPredioResponse response = new UsuarioPredioResponse() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                DataTable datos = Datos.Usuarios.UsuarioPredio.Obtener(id);

                if (datos.Rows.Count > 0)
                {
                    foreach (DataRow row in datos.Rows)
                    {
                        usuario.Id = Convert.ToInt32(row["id"]);
                        usuario.NroDocumento = row["nroDocumento"].ToString();
                        usuario.Email = row["email"].ToString();
                        usuario.Password = row["password"].ToString();
                        usuario.FechaRegistro = Convert.ToDateTime(row["registro_fecha"]);
                        usuario.Longitud = row["coords_lg"].ToString();
                        usuario.Latitud = row["coords_lt"].ToString();
                    }

                    response.Resultado = Tuneles.StatusCode.OK;

                }
                else
                {
                    usuario = null;
                    response.Resultado = Tuneles.StatusCode.WARNING;
                    response.Mensaje = "No se encontró el usuario solicitado";
                }

            }
            catch (Exception ex)
            {
                usuario = null;
                response.Mensaje = ex.Message;
            }

            response.Contenido = usuario;
            return response;

        }
        #endregion

        #region Metodos Privados
        private bool ValidarRegistro(out string? mensaje)
        {
            StringBuilder mensajes = new();


            bool EMAIL_OK = false;
            string mensajeMail = String.Empty;
            if (Email != null)
            {
                EMAIL_OK = Email.ValidarEmail(out string? msgOutMail);
                mensajeMail = msgOutMail ?? String.Empty;
            }
            bool TELEFONO_OK = ValidarTelefono(out string? mensajeTelefono);
            bool LATITUD_OK = ValidarCoordenadas("latitud", out string? mensajeLatitud);
            bool LONGITUD_OK = ValidarCoordenadas("longitud", out string? mensajeLongitud);

            if (EMAIL_OK && TELEFONO_OK && LATITUD_OK && LONGITUD_OK)
            {
                mensaje = null;
                return true;
            }
            else
            {
                mensajes.AppendLine(mensajeMail);
                mensajes.AppendLine(mensajeTelefono);
                mensajes.AppendLine(mensajeLatitud);
                mensajes.AppendLine(mensajeLongitud);


                mensaje = mensajes.ToString();
                return false;
            }
        }

  
        private bool ValidarCoordenadas(string tipo, out string? mensaje)

        {
            mensaje = null;
            if (tipo == "latitud")
            {
                if(String.IsNullOrEmpty(Latitud?.Trim()))
                {
                    mensaje = "No se ha establecido la latitud";
                    return false;
                }
                return true;
            }
            else if (tipo == "longitud")
            {
                if (String.IsNullOrEmpty(Longitud?.Trim()))
                {
                    mensaje = "No se ha establecido la longitud";
                    return false;
                }
                return true;
            }
            else
            {
                mensaje = "No se ha establecido una coordenada valida para el metodo interno [ERROR INTERNO]";
                return false;
            }

        }

        private bool ValidarTelefono(out string? mensaje)
        {
            if (String.IsNullOrEmpty(Telefono))
            {
                mensaje = "No se ha introducido un número de teléfono";
                return false;
            }
            if (Telefono.Length < 6)
            {
                mensaje = "El teléfono introducido es muy corto";
                return false;
            }
            if (Telefono.Length > 15)
            {
                mensaje = "El teléfono introducido es muy largo";
                return false;
            }
            if (!Telefono.EsDigito())
            {
                mensaje = "El teléfono no tiene el formato correcto. Solo se aceptan dígitos";
                return false;
            }

            mensaje = null;
            return true;
        }

        #endregion
    }
}
