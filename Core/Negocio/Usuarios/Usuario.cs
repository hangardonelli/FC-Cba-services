#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Negocio.Turnos;
namespace Core.Negocio.Usuarios
{
    public class Usuario : UsuarioBase
    {
        #region Propiedades públicas
        /// <summary>
        /// Indica el número de teléfono de contacto del usuario
        /// </summary>
        public string? NroTelefono
        {
            get => _NroTelefono;
            set
            {
                if (value == null) _NroTelefono = value;
                else _NroTelefono = value.Trim();
            }
        }
        /// <summary>
        /// Indica el apellido del usuario
        /// </summary>
        public string? Apellido { get; set; }
        /// <summary>
        /// Indica si el usuario está habilitado
        /// </summary>
        public bool Habilitado { get; set; }
        /// <summary>
        /// Indica si el usuario juega al fútbol
        /// </summary>
        public bool EsJugadorFutbol { get; set; }
        /// <summary>
        /// Indica si el usuario juega al paddle
        /// </summary>
        public bool EsJugadorPaddle { get; set; }
        /// <summary>
        /// Indica la posicción de fútbol que aplica el usuario
        /// </summary>
        public PosicionFutbol PosicionFutbol { get; set; } = PosicionFutbol.Indefinido;

        public string? FotoPerfil
        {
            get => "https://direccionfotosusuarioperfil/" + _FotoPerfil;
            set => _FotoPerfil = value;
        }

        public string? DescripcionPersonal { get; set; }
        #endregion

        #region Propiedades privadas
        private string? _NroTelefono { get; set; }
        private string? _FotoPerfil { get; set; }
        #endregion

        #region Metodos públicos

        public UsuarioResponse Modificar()
        {
            UsuarioResponse response = new() { AccionRealizada = AccionRealizada.MODIFICAR };
            try
            {
                #region Chequeo nulls
                if (!Id.HasValue) throw new Exception("No se ha inicializado el usuario");
                if (Nombre == null) throw new Exception("No se ha establecido el nombre para este usuario");
                if (Apellido == null) throw new Exception("No se ha establecido el apellidoe para este usuario");
                if (Email == null) throw new Exception("No se ha establecido una dirección de correo electrónico para este usuario");
                if (NroTelefono == null) throw new Exception("No se ha establecido un número de teléfono para este usuario");
                if (NroDocumento == null) throw new Exception("No se ha establecido un número de documento para este usuario");
                if (Password == null) throw new Exception("No se ha establecido una contraseña para este usuario");
                
                #endregion

                #region Validacion datos
                if (!Email.ValidarEmail(out string? mensajeMail)) throw new Exception(mensajeMail);
                if (NroTelefono.Any(c => !Char.IsDigit(c))) throw new Exception("El documento tiene carácteres no numéricos");
                if (NroTelefono.Length < 6 || NroTelefono.Length > 9) throw new Exception("La longitud del documento es inválida");
                if (NroDocumento.Length < 5 || NroDocumento.Length > 25) throw new Exception("La longitud del teléfono es inválida");
                if (DescripcionPersonal != null && DescripcionPersonal.Length > 200) throw new Exception("La descripción personal es demasiado larga");
               
                #endregion

               
                _ = Datos.Usuarios.Usuario.Modificar(Id.Value, Nombre, Apellido, Email, NroTelefono, Password, NroDocumento, _FotoPerfil, EsJugadorFutbol, (short)PosicionFutbol, EsJugadorPaddle, DescripcionPersonal, Habilitado);
                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;
            }
            catch(Exception ex)
            {
                response.Mensaje = "Se produjo un error al intentar modificar el usuario: " + ex.Message;
                response.Resultado = Tuneles.StatusCode.ERROR;
            }

            return response;
        }
        public TurnoFutbolListaResponse ObtenerTurnos(List<CanchaFutbol> cacheCanchas, List<Predios.Predio> cachePredios)
        {
            TurnoFutbolListaResponse response = new TurnoFutbolListaResponse() { AccionRealizada = Turnos.AccionRealizada.CONSULTAR };
            try
            {
                if (!Id.HasValue) throw new Exception("No se ha inicializado el identificador de usuario");

                List <TurnoFutbol> turnos = new();
                DataTable datos = Datos.Turnos.TurnoFutbol.ObtenerDeUsuario(Id.Value);

                foreach(DataRow row in datos.Rows)
                {
                    TurnoFutbolResponse turnoResponse = TurnoFutbol.Obtener(row, this, cacheCanchas, cachePredios);
                    if (turnoResponse.Resultado != Tuneles.StatusCode.OK || turnoResponse.Contenido == null)
                        throw new Exception(turnoResponse.Mensaje ?? "Se produjo un error desconocido al obtener un turno");

                    turnos.Add(turnoResponse.Contenido);
                }

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = turnos;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Hubo un error al obtener los turnos del usuario: " + ex.Message;
            }

            return response;
        }
        
        /// <summary>
        /// Guarda en base de datos un nuevo usuario a partir de la instancia actual
        /// </summary>
        public UsuarioResponse Crear()
        {

            UsuarioResponse response = new() { AccionRealizada = AccionRealizada.CREAR };
            try
            {
                #region Chequeo nulls
                if (Nombre == null) throw new Exception("No se ha establecido el nombre para este usuario");
                if (Apellido == null) throw new Exception("No se ha establecido el apellidoe para este usuario");
                if (Email == null) throw new Exception("No se ha establecido una dirección de correo electrónico para este usuario");
                if (_NroTelefono == null) throw new Exception("No se ha establecido un número de teléfono para este usuario");
                if (_NroDocumento == null) throw new Exception("No se ha establecido un número de documento para este usuario");
                if (_Password == null) throw new Exception("No se ha establecido una contraseña para este usuario");
                #endregion

                #region Validacion datos
                if (!Email.ValidarEmail(out string? mensajeMail)) throw new Exception(mensajeMail);
                if (_NroTelefono.Any(c => !Char.IsDigit(c))) throw new Exception("El documento tiene carácteres no numéricos");
                if (_NroTelefono.Length < 6 || _NroTelefono.Length > 9) throw new Exception("La longitud del documento es inválida");
                if (_NroDocumento.Length < 5 || _NroDocumento.Length > 25) throw new Exception("La longitud del teléfono es inválida");
                if (DescripcionPersonal != null && DescripcionPersonal.Length > 200) throw new Exception("La descripción personal es demasiado larga");
                #endregion

                DataTable datos = Datos.Usuarios.Usuario.Crear(Nombre, Apellido, Email, _NroTelefono, _Password, _NroDocumento, _FotoPerfil, EsJugadorFutbol, (short)PosicionFutbol, EsJugadorPaddle, DescripcionPersonal);

                #region Validacion post-inserción
                if (datos.Rows.Count < 0 || datos.Rows[0]["id"] == null) throw new Exception("Hubo un error en la inicialización del identificador en base de datos");
                if (!Int32.TryParse(datos.Rows[0]["id"].ToString(), out int r_id)) throw new Exception("Hubo un error en el formato del identificador de usuario");
                if (r_id == 0) throw new Exception("Hubo un error en la inicialización del id del usuario (0)");
                #endregion

                Id = r_id;
                FechaRegistro = DateTime.Now;

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

        /// <summary>
        /// Verificar si la contraseña introducida coincide con la del usuario
        /// </summary>
        public UsuarioResponse VerificarPassword(string passwordPlana)
        {
            UsuarioResponse response = new() { AccionRealizada = AccionRealizada.VALIDARCREDENCIALES };

            try
            {
                if (BCrypt.Net.BCrypt.Verify(passwordPlana, _Password))
                {
                    response.Resultado = Tuneles.StatusCode.OK;
                    response.Contenido = this;
                }
                else throw new InvalidCredentialException();
            }
            catch (InvalidCredentialException)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Las credenciales no son correctas";
                response.CodigoError = "INVALID_CREDENTIALS_USER";
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }


        /// <summary>
        /// Obtiene un usuario por su mail
        /// </summary>
        public static UsuarioResponse Obtener(string mail)
        {
            UsuarioResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                Usuario usuario = new ();
                DataTable usuarioDatos = Datos.Usuarios.Usuario.Obtener(mail);

                if (usuarioDatos.Rows.Count == 0) throw new UserNotFoundException();
                if (usuarioDatos.Rows.Count > 1) throw new AmbiguousUserException();
                foreach (DataRow row in usuarioDatos.Rows)
                {
                    #region Chequeo Nulls
                    if (row["id"] == null) throw new Exception("No se ha cargado programáticamente el id para el usuario");
                    if (row["nombre"] == null) throw new Exception("No se ha cargado programáticamente el nombre para el usuario");
                    if (row["apellido"] == null) throw new Exception("No se ha cargado programáticamente el apellido para el usuario");
                    if (row["email"] == null) throw new Exception("No se ha cargado programáticamente el email para el usuario");
                    if (row["telefono"] == null) throw new Exception("No se ha cargado programáticamente el telefono para el usuario");
                    if (row["fecha_registro"] == null) throw new Exception("No se ha cargado programáticamente la fecha del registro para el usuario");
                    if (row["habilitado"] == null) throw new Exception("No se puede establecer el estado de habilitación del usuario");
                    if (row["juega_futbol"] == null) throw new Exception("No se puede establecer si el usuario juega al fútbol");
                    if (row["futbol_posicion"] == null) throw new Exception("No se puede establecer la posición de fútbol del usuario");
                    if (row["juega_paddle"] == null) throw new Exception("No se puede establecer si el usuario juega al paddle");
                    if (row["descripcion"] == null) throw new Exception("No se pudo obtener la descripción del usuario");
                    if (row["foto_perfil"] == null) throw new Exception("No se ha cargado la foto de perfil del usuario");
                    #endregion

                    #region Chequeo formato
                    if (!Int32.TryParse(row["id"].ToString(), out int r_id)) throw new FormatException("El id del usuario no tiene el formato correcto");
                    if (!Int32.TryParse(row["futbol_posicion"].ToString(), out int r_futbol_posicion)) throw new FormatException("La posición de fútbol del usuario es invalida");
                    if (!Boolean.TryParse(row["habilitado"].ToString(), out bool r_habilitado)) throw new FormatException("La habilitación del usuario no tiene el formato correcto");
                    if (!Boolean.TryParse(row["juega_futbol"].ToString(), out bool r_juega_futbol)) throw new FormatException("No se pudo calcular si el usuario juega fútbol");
                    if (!Boolean.TryParse(row["juega_paddle"].ToString(), out bool r_juega_paddle)) throw new FormatException("No se pudo calcular si el usuario juega paddle");
                    #endregion

                    usuario.Id = r_id;
                    usuario.Nombre = row["nombre"].ToString();
                    usuario.Apellido = row["apellido"].ToString();
                    usuario.Email = row["email"].ToString();
                    usuario.NroTelefono = row["telefono"].ToString();
                    usuario.Password = row["password"].ToString();
                    usuario.FechaRegistro = Convert.ToDateTime(row["fecha_registro"]);
                    usuario.Habilitado = r_habilitado;
                    usuario.PosicionFutbol = (PosicionFutbol)r_futbol_posicion;
                    usuario.EsJugadorFutbol = r_juega_futbol;
                    usuario.EsJugadorPaddle = r_juega_paddle;
                    usuario.DescripcionPersonal = row["descripcion"].ToString();
                    usuario.FotoPerfil = row["foto_perfil"].ToString();

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
        /// Obtiene un usuario de la app por su id
        /// </summary>
        public static UsuarioResponse Obtener(int id)
        {
            UsuarioResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                Usuario usuario = new ();
                DataTable usuarioDatos = Datos.Usuarios.Usuario.Obtener(id);

                if (usuarioDatos.Rows.Count == 0) throw new UserNotFoundException();
                if (usuarioDatos.Rows.Count > 1) throw new AmbiguousUserException();
                foreach (DataRow row in usuarioDatos.Rows)
                {
                    #region Chequeo Nulls
                    if (row["id"] == null) throw new Exception("No se ha cargado programáticamente el id para el usuario");
                    if (row["nombre"] == null) throw new Exception("No se ha cargado programáticamente el nombre para el usuario");
                    if (row["apellido"] == null) throw new Exception("No se ha cargado programáticamente el apellido para el usuario");
                    if (row["email"] == null) throw new Exception("No se ha cargado programáticamente el email para el usuario");
                    if (row["telefono"] == null) throw new Exception("No se ha cargado programáticamente el telefono para el usuario");
                    if (row["fecha_registro"] == null) throw new Exception("No se ha cargado programáticamente la fecha del registro para el usuario");
                    if (row["habilitado"] == null) throw new Exception("No se puede establecer el estado de habilitación del usuario");
                    if (row["juega_futbol"] == null) throw new Exception("No se puede establecer si el usuario juega al fútbol");
                    if (row["futbol_posicion"] == null) throw new Exception("No se puede establecer la posición de fútbol del usuario");
                    if (row["juega_paddle"] == null) throw new Exception("No se puede establecer si el usuario juega al paddle");
                    if (row["descripcion"] == null) throw new Exception("No se pudo obtener la descripción del usuario");
                    if (row["foto_perfil"] == null) throw new Exception("No se pudo obtener la foto de perfil del usuario");

                    #endregion

                    #region Chequeo formato
                    if (!Int32.TryParse(row["id"].ToString(), out int r_id)) throw new FormatException("El id del usuario no tiene el formato correcto");
                    if (!Int32.TryParse(row["futbol_posicion"].ToString(), out int r_futbol_posicion)) throw new FormatException("La posición de fútbol del usuario es invalida");
                    if (!Boolean.TryParse(row["habilitado"].ToString(), out bool r_habilitado)) throw new FormatException("La habilitación del usuario no tiene el formato correcto");
                    if (!Boolean.TryParse(row["juega_futbol"].ToString(), out bool r_juega_futbol)) throw new FormatException("No se pudo calcular si el usuario juega fútbol");
                    if (!Boolean.TryParse(row["juega_paddle"].ToString(), out bool r_juega_paddle)) throw new FormatException("No se pudo calcular si el usuario juega paddle");
                    #endregion

                    usuario.Id = r_id;
                    usuario.Nombre = row["nombre"].ToString();
                    usuario.Apellido = row["apellido"].ToString();
                    usuario.Email = row["email"].ToString();
                    usuario.NroTelefono = row["telefono"].ToString();
                    usuario.Password = row["password"].ToString();
                    usuario.FechaRegistro = Convert.ToDateTime(row["fecha_registro"]);
                    usuario.Habilitado = r_habilitado;
                    usuario.PosicionFutbol = (PosicionFutbol)r_futbol_posicion;
                    usuario.EsJugadorFutbol = r_juega_futbol;
                    usuario.EsJugadorPaddle = r_juega_paddle;
                    usuario.DescripcionPersonal = row["descripcion"].ToString();
                    usuario.FotoPerfil = row["foto_perfil"].ToString();
                }

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = usuario;

            }
            catch (AmbiguousUserException)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se encontró más de un usuario con el mismo ID, no es posible obtenerlo";
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
        #endregion
    }
}
