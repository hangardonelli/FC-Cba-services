#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Core.Negocio.Usuarios
{
    /// <summary>
    /// Clase base para representar a los usuarios de la aplicación
    /// </summary>
    public abstract class UsuarioBase
    {
        #region Propiedades públicas
        /// <summary>
        /// El identificador de usuario
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// El nombre del usuario
        /// </summary>
        public string? Nombre { get; set; }
        /// <summary>
        /// DNI
        /// </summary>
        public string? NroDocumento
        {
            get => _NroDocumento;
            set
            {
                if (value == null) _NroDocumento = value;
                else _NroDocumento = value.Trim();
            }
        }
        /// <summary>
        /// Dirección de correo electrónico
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        public string? Password
        {
            get => _Password;
            set => _Password = value == null || (value.Length < 5 || value.Length > 35) ? null : BCrypt.Net.BCrypt.HashPassword(value);
        }
        /// <summary>
        /// Fecha de creación de la cuenta
        /// </summary>
        public DateTime? FechaRegistro { get; set; }
        #endregion

        #region Propiedades privadas
        protected string? _Password { get; set; }
        protected string? _NroDocumento { get; set; }

        #endregion
    }


    /// <summary>
    /// Excepción que se produce al introducir incorrectamente las credenciales de inicio de sesión
    /// </summary>
    public class InvalidCredentialException : Exception
    {

    }
    /// <summary>
    /// Excepción que se produce al intentar obtener un usuario inexistente
    /// </summary>
    public class UserNotFoundException : Exception
    {

    }
    /// <summary>
    /// Excepción que se produce cuando se encontraron dos usuarios con el mismo identificador
    /// </summary>
    public class AmbiguousUserException : Exception
    {

    }
}
