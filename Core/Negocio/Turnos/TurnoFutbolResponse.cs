using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;
namespace Core.Negocio.Turnos
{
    public enum AccionRealizada
    {
        /// <summary>
        /// Crear un turno
        /// </summary>
        CREAR = 0,
        /// <summary>
        /// Cancelar un turno
        /// </summary>
        CANCELAR = 1,
        /// <summary>
        /// Verificar la disponibilidad de un turno
        /// </summary>
        VERIFICARDISPONIBILIDAD = 2,
        /// <summary>
        /// Traer todos los turnos disponibles
        /// </summary>
        DISPONIBLES = 3,
        /// <summary>
        /// Obtener un turno
        /// </summary>
        CONSULTAR = 4,
        MODIFICAR = 5

    }

    /// <summary>
    /// Respuesta de turno 
    /// </summary>
    public class TurnoFutbolResponse : Respuesta<TurnoFutbol>
    {
        public AccionRealizada AccionRealizada { get; set; }
       
    }
    /// <summary>
    /// Respuestas de turno que requieran listas
    /// </summary>
    public class TurnoFutbolListaResponse : Respuesta<List<TurnoFutbol>>
    {
        public AccionRealizada AccionRealizada { get; set; }

    }
    /// <summary>
    /// Usado para verificar la disponibilidad de un turno
    /// </summary>
    public class TurnoFutbolDisponibilidadResponse : Respuesta<bool>
    {
        public List<string>? CanchasDisponibles { get; set; }
        public List<string>? CanchasReservadas { get; set; }
        public AccionRealizada AccionRealizada { get; set; }

    }
}

