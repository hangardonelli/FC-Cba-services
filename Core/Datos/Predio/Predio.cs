using System.Data;

namespace Core.Datos.Predio
{
    /// <summary>
    /// Represeneta a un predio de FC Cordoba
    /// </summary>
    public class Predio
    {


   
        /// <summary>
        /// Obtener todos los predios de FC - Cordoba
        /// </summary>
        public static DataTable ObtenerTodos()
        {
            return PostgreSQL.pQuery("SELECT * FROM \"Predios\" ", new Dictionary<string, object?>());

        }
        /// <summary>
        /// Obtiene un predio por su id
        /// </summary>
        public static DataTable Obtener(int id) => PostgreSQL
            .pQuery("SELECT p.id, \"nombre\", \"email\", \"usuario\", \"perfil\", \"portada\", \"habilitado\", \"localidad\", \"direccion\", l.id, l.provincia_id, \"descripcion\" FROM \"Predios\" p INNER JOIN \"Localidades\" l ON l.id = p.localidad WHERE p.id = @id",
                new Dictionary<string, object?>()
                {
                    {"id", id}
                });

        /// <summary>
        /// Obtiene todas las canchas principales de un predio
        /// </summary>
        public static DataTable ObtenerCanchas(int idPredio) => PostgreSQL
            .pQuery("SELECT * FROM \"CanchasFutbol\" WHERE \"predio_id\" = @idPredio AND \"principal\" = TRUE",
                new Dictionary<string, object?>()
                {
                    {"idPredio", idPredio}
                }
            );

        /// <summary>
        /// Crea un predio
        /// </summary>
        public static DataTable Crear(string nombre, string email, int usuario, string? perfil, string? portada, int localidad, string direccion) => PostgreSQL
            .pQuery("INSERT INTO \"Predios\" (\"nombre\", \"email\", \"usuario\", \"perfil\", \"portada\", \"habilitado\", \"localidad\", \"direccion\") VALUES (@nombre, @email, @usuario, @perfil, @portada, FALSE, @localidad, @direccion);SELECT currval(pg_get_serial_sequence('\"Predios\"','id'))  AS id;",
                new Dictionary<string, object?>()
                {
                    {"nombre", nombre},
                    {"email", email},
                    {"usuario", usuario},
                    {"perfil", perfil },
                    {"portada", portada},
                    {"localidad", localidad},
                    {"direccion", direccion}
                }
            );
        /// <summary>
        /// Modifica algún dato de un predio (NO INCLUYE PREFERENCIAS)
        /// </summary>
        /// <returns></returns>
        public static DataTable Modificar(int id, string nombre, string email, string? perfil, string? portada, int localidad, string direccion) => PostgreSQL
            .pQuery("UPDATE \"Predios\" SET \"nombre\" = @nombre, \"email\" = @email, \"perfil\" = @perfil, \"portada\" = @portada, \"localidad\" = @localidad, direccion = @direccion WHERE \"id\" = @id",
                new Dictionary<string, object?>()
                {
                    {"nombre", nombre },
                    {"email", email },
                    {"perfil", perfil },
                    {"portada", portada },
                    {"localidad", localidad },
                    {"id", id },
                    {"direccion", direccion},
                }
             );
    }
}
