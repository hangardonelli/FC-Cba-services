using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Core.Datos.Usuarios
{
    public class UsuarioPredio
    {

        /// <summary>
        /// Obtiene un usuario por su id
        /// </summary>
        public static DataTable Obtener(int id) => PostgreSQL
            .pQuery("SELECT * FROM \"UsuariosPredio\" WHERE id = @id",
                new Dictionary<string, object?>()
                {
                    {"id", id}
                }
            );

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        public static DataTable Obtener(string email) => PostgreSQL
            .pQuery("SELECT * FROM \"UsuariosPredio\" WHERE \"email\" = @email",
                new Dictionary<string, object?>()
                {
                    {"email", email}
                }
            );

        /// <summary>
        /// Crea un usuario a partir de su email y contraseña
        /// </summary>
        public static DataTable Crear(string email, string password, string nroDocumento, string latitud, string longitud, string telefono) => PostgreSQL
            .pQuery("INSERT INTO \"UsuariosPredio\" (\"email\", \"password\", \"registro_fecha\", \"ultimaconexion\", \"habilitado\", \"nrodocumento\", \"coords_lt\", \"coords_lg\", \"telefono\") VALUES (@email, @password, CURRENT_TIMESTAMP, NULL, FALSE, @nroDocumento, @latitud, @longitud, @telefono);SELECT currval(pg_get_serial_sequence('\"UsuariosPredio\"','id')) AS id;",
                new Dictionary<string, object?>()
                {
                    {"email", email},
                    {"password", password},
                    {"nroDocumento", nroDocumento},
                    {"latitud", latitud},
                    {"longitud", longitud },
                    {"telefono", telefono }
                }
            );
    }
}
