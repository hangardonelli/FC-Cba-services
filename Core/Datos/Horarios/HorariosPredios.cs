using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Core.Datos.Horarios
{
    public class HorariosPredios
    {
        /// <summary>
        /// Obtiene un DataTable con todos los horarios cargados para el predio
        /// </summary>
        public static DataTable ObtenerPorPredio(int predio_id)
        {
            return PostgreSQL.pQuery("SELECT * FROM \"HorariosPredios\" WHERE \"predio_id\" = @predio_id",
                new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id},
                });
        }

        /// <summary>
        /// Modifica un horario predio
        /// </summary>
        public static DataTable Modificar(int predio_id, DayOfWeek dia_semana_id, decimal hora)
        {
            return PostgreSQL.pQuery("SELECT * FROM actualizarHorarioPredio(@predio_id, CAST(@dia_semana_id AS SMALLINT), CAST(@hora AS NUMERIC(4,2))));",
                new Dictionary<string, object?>()
                {
                    { "predio_id", predio_id },
                    { "dia_semana_id", (int)dia_semana_id },
                    { "hora", hora },
                });
        }

        /// <summary>
        /// Inserta en base de datos el horario de turno de un predio
        /// </summary>
        public static DataTable Crear(int predio_id, DayOfWeek dia_semana_id, decimal hora)
        {
            return PostgreSQL.pQuery("SELECT * FROM insertarHorarioPredio(@predio_id, CAST(@dia_semana_id AS SMALLINT), CAST(@hora AS DECIMAL(4,2));",
                new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id },
                    {"dia_semana_id", (int)dia_semana_id },
                    {"hora", hora }
                });
        }

        /// <summary>
        /// Elimina de base de datos el horario de turno de un predio
        /// </summary>
        public static DataTable Eiminar(int predio_id, DayOfWeek dia_semana_id, decimal hora)
        {
            return PostgreSQL.pQuery("SELECT * FROM EliminarHorarioPredio(@predio_id, CAST(@dia_semana_id AS SMALLINT), CAST(@hora AS DECIMAL(4,2));",
                 new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id },
                    {"dia_semana_id", (int)dia_semana_id },
                    {"hora", hora }
                });
        }
    }
}
