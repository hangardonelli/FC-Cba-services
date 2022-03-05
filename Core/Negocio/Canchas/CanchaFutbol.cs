using Core.Negocio.Canchas;
using Core.Negocio.Predios;
using System.Data;

namespace Core.Negocio
{
    /// <summary>
    /// Representa a una cancha de FC Cordoba
    /// </summary>
    public class CanchaFutbol
    {
        #region Propiedades publicas
        /// <summary>
        /// El íd de la cancha
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// El predio  dueño de la cancha
        /// </summary>
        public Predio? Predio { get; set; }
        /// <summary>
        /// El valor del turno de la cancha
        /// </summary>
        public int Precio { get; set; }
        /// <summary>
        /// Las divisiones (canchas hijas)
        /// </summary>
        public List<CanchaFutbol> Divisiones { get; set; } = new List<CanchaFutbol>();
        /// <summary>
        /// La cantidad de jugadores
        /// </summary>
        public int NroJugadores { get; set; }

        /// <summary>
        /// Indica si la cancha va a aparecer en la app o no
        /// </summary>
        public bool Habilitada { get; set; } = false;

        /// <summary>
        /// Indica si la cancha es una cancha principal (no tiene padres)
        /// </summary>
        public bool Principal { get; set; }

        /// <summary>
        /// Indica el tipo de cesped de la cancha
        /// </summary>
        public TipoCesped TipoCesped { get; set; }
        #endregion
        #region Propiedades Privadas

        #endregion
        #region Metodos publicos



        /// <summary>
        /// Obtiene una cancha por su id
        /// </summary>
        public static CanchaResponse Obtener(string idCancha, Predio predio)
        {
            CanchaResponse response = new ();
            response.AccionRealizada = Canchas.AccionRealizada.OBTENER;
            try
            {
                DataTable datos = Datos.Cancha.CanchaFutbol.Obtener(idCancha, predio.Id.Value);
                if(datos.Rows.Count > 0)
                {
                    CanchaFutbol cancha = ObtenerCancha(idCancha, predio, datos);
                    response.Contenido = cancha;
                    response.Adicional = cancha.ToXML();
                }
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
                response.Adicional = ex.StackTrace;
            }

            return response;
        }


        /// <summary>
        /// Elimina la instancia de la cancha de la base de datos
        /// </summary>
        /// <exception cref="NullReferenceException">Se produce cuando el ID de la cancha o el ID del predio es NULL</exception>
        public CanchaResponse Eliminar()
        {
            CanchaResponse response = new () { AccionRealizada = Canchas.AccionRealizada.ELIMINARDIVISION };
            try
            {
                ChequearNulos();

                Datos.Cancha.CanchaFutbol.Eliminar(Id, Predio.Id);
                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }
            response.Adicional = this.ToXML();
            return response;
        }

        /// <summary>
        /// Agrega una cancha
        /// </summary>
        public CanchaResponse Agregar()
        {
            CanchaResponse response = new () { AccionRealizada = Canchas.AccionRealizada.AGREGARDIVISION };

            try
            {
                if (Predio == null || Predio.Id == null)
                {
                    response.CodigoError = "PREDIO_NULL";
                    throw new NullReferenceException("No se ha establecido un predio a esta cancha");
                }
                if(!(TipoCesped == TipoCesped.Natural || TipoCesped == TipoCesped.Sintetico))
                {
                    response.CodigoError = "TIPOCESPED_NULL";
                    throw new NullReferenceException("No se ha establecido un tipo de cesped a esta cancha");
                }

                Datos.Cancha.CanchaFutbol.Agregar(Id, Precio, NroJugadores, (int)TipoCesped, Predio.Id.Value, Principal, Habilitada);
                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;
            }

            //Errores del lado del servidor de PostgreSQL:
            catch (Npgsql.PostgresException ex)
            {
                switch (ex.SqlState)
                {
                    case "23505": //Violación Cancha.Id y Predio.Id repetidos:
                        response.Mensaje = "Este predio ya tiene una cancha con el mismo id";
                        response.CodigoError = "CANCHA_EXISTE";
                        break;
                    default:
                        response.Mensaje = ex.Message;
                        break;
                }
                response.Resultado = Tuneles.StatusCode.ERROR;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            response.Adicional = this.ToXML();
            return response;
        }

        /// <summary>
        /// Guarda las modificaciones de la instancia actual de Cancha en la base de datos
        /// </summary>
        public CanchaResponse Modificar()
        {
            CanchaResponse response = new () { AccionRealizada = Canchas.AccionRealizada.MODIFICAR };
           
            try
            {
                ChequearNulos();
                Datos.Cancha.CanchaFutbol.Modificar(Id, Precio, NroJugadores, (int)TipoCesped, Predio.Id.Value, Habilitada);
                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;
            }
            catch(Exception ex)
            {
                response.Mensaje = ex.Message;
                response.Resultado = Tuneles.StatusCode.ERROR;
            }

            return response;
        }
        #endregion

        #region Metodos Privados
        private void ChequearNulos()
        {
            if (Id == null)
                throw new NullReferenceException("No se ha establecido el ID de la cancha");
            if (Predio == null)
                throw new NullReferenceException("No se ha establecido el predio de la cancha");
            if (Predio.Id == null)
                throw new NullReferenceException("No se ha establecido el id del predio de la cancha");
        }
        private static CanchaFutbol ObtenerCancha(string id, Predio predio, DataTable datos, bool init = true, CanchaFutbol canchaRec = null)
        {

            CanchaFutbol canchaActual = canchaRec;
            if (canchaRec == null)
            {
                canchaActual = new();
                canchaActual.Id = id;
                canchaActual.Predio = predio;
            }
            foreach (DataRow row in datos.Rows)
            {
                CanchaFutbol canchaTemp = new ()
                {
                    Id = row["id"].ToString(),
                    NroJugadores = Convert.ToInt32(row["nroJugadores"]),
                    Precio = Convert.ToInt32(row["Precio"]),
                    TipoCesped = Convert.ToInt32(row["cesped"]) == 1 ? TipoCesped.Natural : TipoCesped.Sintetico,
                    Habilitada = Convert.ToBoolean(row["habilitada"]),
                    Principal = Convert.ToBoolean(row["principal"]),
                    Predio = predio
                };
                if (row["id"].ToString().Split('.').Length == (id.Split('.').Length + 1) && row["id"].ToString().StartsWith(id))
                    canchaActual.Divisiones.Add(canchaTemp);

                else if (init && row["id"].ToString() == id)
                    canchaActual = canchaTemp;
            }


            foreach (CanchaFutbol cancha in canchaActual.Divisiones)
                ObtenerCancha(cancha.Id, predio, datos, false, cancha);

            return canchaActual;


        }
        #endregion

    }

    public class NroJugadoresException : Exception
    {
        public new string Message = "La cantidad de jugadores ingresada no es valida.";
    }
}
