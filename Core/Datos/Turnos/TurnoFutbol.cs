using System.Data;

namespace Core.Datos.Turnos
{
    public class TurnoFutbol
    {

        public static DataTable Calificar(int turno_id, int calificacion, string? mensaje)
        {
            var parametros = new Dictionary<string, object?>()
            {
                {"id", turno_id },
                {"calificacion", calificacion},
                {"mensaje", mensaje},
            };

            return PostgreSQL.pQuery(@"
                INSERT INTO ""ResenasFutbol"" VALUES
                (@id, @calificacion, @mensaje, CURRENT_TIMESTAMP);
                ", parametros);
        }
        /// <summary>
        /// Modifica un turno
        /// </summary>
        public static DataTable Modificar(int id, int estado, int metodo_pago, int tipo_cesped, decimal hora)
        {
            return PostgreSQL.pQuery("SELECT * FROM modificarTurnoFutbol(@id, CAST(@estado AS SMALLINT), CAST(@metodo_pago AS SMALLINT), CAST(@tipo_cesped AS SMALLINT), CAST(@hora AS DECIMAL(4, 2)));",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                    {"estado", estado},
                    {"metodo_pago", metodo_pago},
                    {"tipo_cesped", tipo_cesped},
                    {"hora", hora},
                });
        }
        public static DataTable ObtenerDeUsuario(int usuario)
        {
            return PostgreSQL.pQuery("SELECT * FROM \"TurnosFutbol\" WHERE \"usuario\" = @usuario",
                new Dictionary<string, object?>()
                {
                    {"usuario", usuario},
                });
        }
        /// <summary>
        /// Obtiene todos los turnos reservados en un rango temporal especificado
        /// </summary>
        public static DataTable ObtenerTurnos(DateOnly fecha, decimal horaDesde, decimal horaHasta)
        {
            return PostgreSQL.pQuery("SELECT * FROM getReservadosFutbolRango(CAST(@fecha AS DATE), @horaDesde, @horaHasta);",
                    new Dictionary<string, object?>()
                    {
                        {"fecha", fecha.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))},
                        {"horaDesde", horaDesde },
                        {"horaHasta", horaHasta }
                    });
        }
        /// <summary>
        /// Obtiene los ids de las canchas reservadas en el tiempo indicado para el predio indicado
        /// </summary>
        public static DataTable ObtenerReservados(string cancha_id, int predio_id, DateOnly fecha, decimal hora) => PostgreSQL
            .pQuery("SELECT * FROM getReservadosFutbol(@cancha_id, @predio_id, CAST(@fecha AS DATE), @hora)",
                new Dictionary<string, object?>()
                {
                    {"cancha_id", cancha_id },
                    {"predio_id", predio_id },
                    {"fecha", fecha },
                    {"hora", hora }
                });

        /// <summary>
        /// Guarda un turno en base de datos
        /// </summary>
        public static DataTable Crear(DateTime fecha, decimal hora, int predio_id, short estado, short metodo_pago, string id_cancha, int tipo_cesped, int precio, int usuario) => PostgreSQL
            .pQuery("INSERT INTO \"TurnosFutbol\" VALUES (@fecha, @hora, @predio_id, @estado, @metodo_pago, NULL, @id_cancha, @tipo_cesped, @precio)",
                new Dictionary<string, object?>() 
                {
                    {"fecha", fecha},
                    {"hora", hora},
                    {"predio_id", predio_id },
                    {"estado", estado},
                    {"metodo_pago", metodo_pago},
                    {"id_cancha", id_cancha },
                    {"tipo_cesped", tipo_cesped},
                    {"precio", precio},
                    {"usuario", usuario }
                });
    }
}
