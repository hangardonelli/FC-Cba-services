using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Core.Datos.Publicidades
{
    public class Publicidad
    {

        /// <summary>
        /// Inserta en la base de datos una nueva publicidad
        /// </summary>
        public static DataTable Crear(string nombre, string archivo, int tipo, string resolucion, DateTime? fecha_expiracion)
        {
            return PostgreSQL.pQuery(@"
                INSERT INTO ""Publicidades""
                VALUES
                (@nombre, @archivo, @tipo, @resolucion, CURRENT_TIMESTAMP, @fecha_expiracion);
                
                SELECT currval(pg_get_serial_sequence('""Publicidades""','id'))  AS id;
                ", new Dictionary<string, object?>()
            {
                {"nombre", nombre},
                {"archivo", archivo},
                {"tipo", tipo  },
                {"resolucion", resolucion },
                {"fecha_expiracion", fecha_expiracion.HasValue ? fecha_expiracion.Value : DBNull.Value},
            });
        }

        /// <summary>
        /// Modifica una publicidad
        /// </summary>
        public static DataTable Modificar(int id, string nombre, string archivo, int tipo, string resolucion, DateTime? fecha_expiracion)
        {
            return PostgreSQL.pQuery(@"
                UPDATE ""Publicidades"" 
                SET 
                    ""nombre"" = @nombre,
                    ""archivo"" = @archivo,
                    ""tipo"" = @tipo,
                    ""resolucion"" = @resolucion,
                    ""fecha_expiracion"" = @fecha_expiracion

                WHERE ""id"" = @id
            ", new Dictionary<string, object?>()
            {
                {"id", id },
                {"nombre", nombre},
                {"archivo", archivo},
                {"tipo", tipo},
                {"fecha_expiracion", fecha_expiracion},
                {"resolucion", resolucion},
            });
        }
        /// <summary>
        /// Elimina una publicidad por su id
        /// </summary>
        /// <returns></returns>
        public static DataTable EliminarPublicidad(int id)
        {
            return PostgreSQL.pQuery(@"DELETE FROM ""Publicidades"" WHERE id = @id;",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                });
        }

        public static DataTable Obtener(int id)
        {
            return PostgreSQL.pQuery(@"SELECT * FROM ""Publicidades"" WHERE id = @id;",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                });
        }
        /// <summary>
        /// Obtiene todas las publicidades de la base de datos
        /// </summary>
        public static DataTable ObtenerTodas()
        {
            return PostgreSQL.Query("SELECT * FROM \"Publicidades\"");
        }
    }
}
