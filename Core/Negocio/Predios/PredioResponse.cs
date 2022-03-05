using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;

namespace Core.Negocio.Predios
{
    public class PredioResponse : Respuesta<Predio>
    {
        /// <summary>
        /// Es la acción realizada de la respuesta específicamente para los predios
        /// </summary>
        public AccionRealizada AccionRealizada { get; set; }
    }

    public class PredioListaResponse : Respuesta<List<Predio>>
    {
        /// <summary>
        /// Es la acción realizada de la respuesta específicamente para los predios
        /// </summary>
        public AccionRealizada AccionRealizada { get; set; }
    }
    public enum AccionRealizada
    {
        /// <summary>
        /// Crear un predio
        /// </summary>
        CREAR = 0,
        /// <summary>
        /// Agregar una cancha
        /// </summary>
        ELIMINAR = 3,
        /// <summary>
        /// Habilitar el predio
        /// </summary>
        HABILITAR = 4,
        /// <summary>
        /// Deshabilitar el predio
        /// </summary>
        DESHABILITAR = 5,
        /// <summary>
        /// Denota cualquier tipo de modificación del predio
        /// </summary>
        MODIFICAR = 6,
        /// <summary>
        /// Al consultar un predio
        /// </summary>
        CONSULTAR = 7,
        /// <summary>
        /// Al consultar las  canchas de un predio
        /// </summary>
        CONSULTARCANCHA = 8
    }

    public class PredioCanchaResponse : Respuesta<List<CanchaFutbol>>
    {
        /// <summary>
        /// Es la acción realizada de la respuesta específicamente para las canchas de los predios
        /// </summary>
        public AccionRealizada AccionRealizada { get; set; }
    }


}
