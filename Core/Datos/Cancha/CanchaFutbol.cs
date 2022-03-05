using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Core.Datos.Cancha
{
    public class CanchaFutbol
    {
        /// <summary>
        /// Obtiene las canchas de un predio
        /// </summary>
        public static DataTable ObtenerPorPredio(int predioId) => PostgreSQL
            .Query($"SELECT * FROM \"CanchasFutbol\" WHERE predio_id = {predioId}");


        public static DataTable Obtener(string id, int predioId) => PostgreSQL
           .pQuery("SELECT * FROM \"CanchasFutbol\" WHERE starts_with(id, @id) AND predio_id = @predioId",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                    {"predioId", predioId}
                });

        /// <summary>
        /// Agrega una cancha a un predio específico
        /// </summary>
        public static DataTable Agregar(string id, int precio, int nroJugadores, int cesped, int predio_id, bool principal, bool habilitada) => PostgreSQL
            .pQuery("INSERT INTO \"CanchasFutbol\" VALUES (@id, @precio, @nroJugadores, @cesped, @predio_id, @principal, @habilitada)",
                new Dictionary<string, object?>()
                {
                    {"id", id },
                    {"precio", precio },
                    {"nroJugadores", nroJugadores },
                    {"cesped", cesped },
                    {"predio_id", predio_id },
                    {"principal", principal },
                    {"habilitada", habilitada }
                }
            );

        /// <summary>
        /// Elimina una cancha
        /// </summary>
        public static DataTable Eliminar(string id, int? idPredio) => PostgreSQL
            .pQuery("DELETE FROM \"CanchasFutbol\" WHERE starts_with(id, @id) AND predio_id = @predio_id",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                    {"predio_id", idPredio}
                }
            );


        public static DataTable Modificar(string id, int precio, int nroJugadores, int cesped, int idPredio, bool habilitada) => PostgreSQL
            .pQuery("UPDATE \"CanchasFutbol\" SET \"Precio\" = @precio, \"nroJugadores\" = @nroJugadores, \"cesped\" = @cesped, \"habilitada\" = @habilitada WHERE id = @id AND predio_id = @idPredio",
                new Dictionary<string, object?>()
                {
                    {"id", id },
                    {"idPredio", idPredio },
                    {"Precio", precio },
                    {"nroJugadores", nroJugadores },
                    {"cesped", cesped },
                    {"habilitada", habilitada }
                }
            );
    }
}
