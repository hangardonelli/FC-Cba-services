using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;

namespace Core.Negocio.Canchas
{
    public class CanchaResponse : Respuesta<CanchaFutbol>
    {
        /// <summary>
        /// Es la acción realizada de la respuesta específicamente para las canchas
        /// </summary>
        public AccionRealizada AccionRealizada { get; set; }
    }

    public enum AccionRealizada
    {
        /// <summary>
        /// Crear una cancha
        /// </summary>
        CREAR = 0,
        /// <summary>
        /// Agregar una división a una cancha
        /// </summary>
        AGREGARDIVISION = 1,
        /// <summary>
        /// Eliminar una división
        /// </summary>
        ELIMINARDIVISION = 2,
        /// <summary>
        /// Eliminar la cancha
        /// </summary>
        ELIMINAR = 3,
        /// <summary>
        /// Habilitar la cancha
        /// </summary>
        HABILITAR = 4,
        /// <summary>
        /// Deshabilitar la cancha
        /// </summary>
        DESHABILITAR = 5,
        /// <summary>
        /// Al obtener una/varias canchas
        /// </summary>
        OBTENER = 6,
        /// <summary>
        /// Al modificar la cancha
        /// </summary>
        MODIFICAR = 7,

    }
}
