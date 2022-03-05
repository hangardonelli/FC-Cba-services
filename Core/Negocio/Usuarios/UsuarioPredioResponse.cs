using Core.Tuneles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Negocio.Usuarios

{
    public class UsuarioPredioResponse : Respuesta<UsuarioPredio>
    {
        /// <summary>
        /// Es la acción realizada de la respuesta específicamente para de los usuarios
        /// </summary>
        public AccionRealizada AccionRealizada { get; set; }
    }

    public enum AccionRealizada
    {
        /// <summary>
        /// Crear un usuario
        /// </summary>
        CREAR = 0,
        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        ELIMINAR = 3,
        /// <summary>
        /// Habilitar el usuario
        /// </summary>
        HABILITAR = 4,
        /// <summary>
        /// Deshabilitar el usuario
        /// </summary>
        DESHABILITAR = 5,
        /// <summary>
        /// Denota cualquier tipo de modificación del usuario
        /// </summary>
        MODIFICAR = 6,
        /// <summary>
        /// Al consultar un usuario
        /// </summary>
        CONSULTAR = 7,
        /// <summary>
        /// Al validar las credenciales
        /// </summary>
        VALIDARCREDENCIALES = 8,
    }
}
