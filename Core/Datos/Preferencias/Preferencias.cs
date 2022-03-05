using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Core.Datos.Preferencias
{
    public class Preferencias
    {
        public static DataTable Crear(int predio_id, int preferencia_id)
        {
            return PostgreSQL.pQuery("SELECT * FROM insertarPreferencia(@predio_id, @preferencia_id);", 
                new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id},
                    {"preferencia_id", preferencia_id},
                });
        }
        public static DataTable Eliminar(int predio_id, int preferencia_id)
        {
            return PostgreSQL.pQuery("SELECT * FROM eliminarPreferencia(@predio_id, @preferencia_id);",
                new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id},
                    {"preferencia_id", preferencia_id},
                });
        }
        public static DataTable ObtenerPreferencia(int predio_id)
        {
            return PostgreSQL.pQuery("SELECT \"preferencia_id\" FROM \"PrediosPreferencias\" WHERE \"predio_id\" = @predio_id",
                new Dictionary<string, object?>()
                {
                    {"predio_id", predio_id},
                });
        }
    }
}
