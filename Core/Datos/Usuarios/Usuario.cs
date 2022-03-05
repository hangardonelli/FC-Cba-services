using System.Data; 

namespace Core.Datos.Usuarios
{
    public class Usuario
    {

        /// <summary>
        /// Modifica los datos personales de un usuario
        /// </summary>
        public static DataTable Modificar(int id, string nombre, string apellido, string email, string telefono, string password, string documento, string? foto_perfil, bool juega_futbol, short posicion_futbol, bool juega_paddle, string? descripcion, bool habilitado)
        {
            return PostgreSQL.pQuery("UPDATE \"Usuarios\" SET \"nombre\" = @nombre, \"apellido\" = @apellido, \"email\" = @email, \"telefono\" = @telefono, \"password\" = @password, \"documento\" = @documento, \"foto_perfil\" = @foto_perfil,  \"juega_futbol\" = @juega_futbol, \"posicion_futbol\" = @posicion_futbol, \"juega_paddle\" = @posicion_paddle\", \"descripcion\" = @descripcion, \"habilitado\" = @habilitado WHERE \"id\" = @id;",
                new Dictionary<string, object?>()
                {
                    {"id", id},
                    {"nombre", nombre},
                    {"apellido", apellido},
                    {"email", email },
                    {"telefono", telefono },
                    {"password", password},
                    {"documento", documento },
                    {"foto_perfi", foto_perfil },
                    {"juega_futbol", juega_futbol },
                    {"posicion_futbol", posicion_futbol },
                    {"juega_paddle", juega_paddle },
                    {"descripcion", descripcion },
                    {"habilitado", habilitado},
                });
        }
        /// <summary>
        /// Inserta en base de datos el nuevo usuario
        /// </summary>
        public static DataTable Crear(string nombre, string apellido, string email, string telefono, string password, string documento, string? foto_perfil, bool juega_futbol, short posicion_futbol, bool juega_paddle, string descripcion) => PostgreSQL
            .pQuery("INSERT INTO \"Usuarios\"(\"nombre\" ,\"apellido\", \"email\", \"telefono\", \"password\", \"fecha_registro\", \"habilitado\", \"documento\", \"foto_perfil\", \"juega_futbol\", \"posicion_futbol\", \"juega_paddle\", \"descripcion\") VALUES (@nombre, @apellido, @email, @telefono, @password, CURRENT_TIMESTAMP, FALSE, @documento, @foto_perfil, @juega_futbol, @posicion_futbol, @juega_paddle, @descripcion); SELECT currval(pg_get_serial_sequence('\"Usuarios\"','id'))  AS id;",
                    new Dictionary<string, object?>()
                    {
                        {"nombre", nombre },
                        {"apellido", apellido},
                        {"email", email},
                        {"telefono", telefono },
                        {"password", password},
                        {"documento", documento},
                        {"foto_perfil", foto_perfil},
                        {"juega_futbol", juega_futbol },
                        {"posicion_futbol", posicion_futbol },
                        {"juega_paddle", juega_paddle },
                        {"descripcion", descripcion }
                    });

        /// <summary>
        /// Obtiene de la base de datos un usuario por su mail
        /// </summary>
        public static DataTable Obtener(string email) => PostgreSQL
            .pQuery("SELECT * FROM \"Usuarios\" WHERE \"email\" = @email", new Dictionary<string, object?>() { { "email", email } });





        /// <summary>
        /// Obtiene de la base de datos un usuario por su ID
        /// </summary>
        public static DataTable Obtener(int id) => PostgreSQL
            .pQuery("SELECT * FROM \"Usuarios\" WHERE \"id\" = @id",
                   new Dictionary<string, object?>()
                   {
                        {"id", id },
                   });
    }
}
