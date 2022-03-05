using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System.Collections;

namespace Core.Datos
{
    /// <summary>
    /// Wrapper de consultas a PostgreSQL
    /// </summary>
    public class PostgreSQL
    {
        internal static string connString = Entorno.Urls.ConexionSQL;

        /// <summary>
        /// Ejecuta una consulta
        /// </summary>
        /// <param name="q">La consulta SQL Plana a ejecutar</param>
        public static DataTable Query(string q)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand(q, conn);
            using var reader = cmd.ExecuteReader();
            DataTable? retorno = new DataTable();
            if (retorno != null)
                retorno.Load(reader);
            return retorno ?? new DataTable();
        }

        /// <summary>
        /// Ejecuta una consulta parametrizada
        /// </summary>
        /// <param name="q">La consulta SQL Plana a ejecutar</param>

        public static DataTable pQuery(string q, Dictionary<string, object?> parametros)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand(q, conn))
            {
                foreach(var param in parametros)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                using var reader = cmd.ExecuteReader();
                DataTable? retorno = new DataTable();
                if (retorno != null)
                    retorno.Load(reader);
                return retorno ?? new DataTable();
            }
        }
    }
}
