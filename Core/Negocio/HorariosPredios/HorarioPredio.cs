#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Negocio.Predios;
namespace Core.Negocio.HorariosPredios
{
    /// <summary>
    /// Representa los horarios de apertura de cada predio para cada día correspondientemente
    /// </summary>
    public class HorarioPredio
    {
        #region Propiedades públicas
        /// <summary>
        /// Representa el predio asociado a este horario
        /// </summary>
        public Predio? Predio { get; set; }
        /// <summary>
        /// Representa un día de la semana en el cuál se configura el horario
        /// </summary>
        public DayOfWeek? DiaSemana { get; set; }

        /// <summary>
        /// Representa la hora de inicio del turno
        /// </summary>
        public decimal? Hora { get; set; }
        #endregion

        #region Metodos públicos

        /// <summary>
        /// Elimina el horario actual de la base 
        /// </summary>
        /// <returns></returns>
        public HorarioPredioResponse Eliminar()
        {
            HorarioPredioResponse response = new() { AccionRealizada = AccionRealizada.ELIMINAR };
            try
            {
                #region Chequeo nulls
                if (Predio == null || Predio.Id == null) throw new Exception("No se pudo inicializar el predio");
                if (!DiaSemana.HasValue) throw new Exception("No se pudo inicializar el día del turno");
                if (!Hora.HasValue) throw new Exception("No se pudo inicializar el horario del turno");
                #endregion

                Datos.Horarios.HorariosPredios.Eiminar(Predio.Id.Value, DiaSemana.Value, Hora.Value);

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Hubo un error al eliminar el horario de un predio: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Crea un horario de turno para un predio específico
        /// </summary>
        public HorarioPredioResponse Crear()
        {
            HorarioPredioResponse response = new() { AccionRealizada = AccionRealizada.CREAR };
            try
            {
                #region Chequeo nulls
                if (Predio == null || Predio.Id == null) throw new Exception("No se pudo inicializar el predio");
                if (!DiaSemana.HasValue) throw new Exception("No se pudo inicializar el día del turno");
                if (!Hora.HasValue) throw new Exception("No se pudo inicializar el horario del turno");
                #endregion

                Datos.Horarios.HorariosPredios.Crear(Predio.Id.Value, DiaSemana.Value, Hora.Value);

                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un error al crear el horario del predio: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Modifica el horario de apertura y cierre para un día de la semana de un predio específico
        /// </summary>
        public HorarioPredioResponse Modificar()
        {
            HorarioPredioResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                #region Chequeo nulls
                if (Predio == null || Predio.Id == null) throw new Exception("No se ha cargado el predio");
                if (DiaSemana == null) throw new Exception("No se ha cargado el día de la semana del horario");
                if (!Hora.HasValue) throw new Exception("No se ha establecido un horario para el turno");
                #endregion

                Datos.Horarios.HorariosPredios.Modificar(Predio.Id.Value, DiaSemana.Value, Hora.Value);

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = this;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.CodigoError = "MODIFICAR_HORARIO_PREDIO_ERROR";
                response.Mensaje = "Error al modificar el horario del predio: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Obtiene los horarios de apertura de un predio en cuestion
        /// </summary>
        public static HorarioPredioListaResponse Obtener(Predio predio)
        {
            HorarioPredioListaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                List<HorarioPredio> horarios = new();
                #region Chequeo nulls
                if (!predio.Id.HasValue) throw new Exception("Error al obtener el horario, el predio no se ha inicializado");
                #endregion
                DataTable datos = Datos.Horarios.HorariosPredios.ObtenerPorPredio(predio.Id.Value);

                foreach (DataRow dr in datos.Rows)
                    horarios.Add(Obtener(dr, predio));

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = horarios;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Obtiene un horario de un predio por un DataRow
        /// </summary>
        internal static HorarioPredio Obtener(DataRow dr, Predio? predioCache = null)
        {

            #region Chequeo nulls
            if (dr["predio_id"] == null) throw new Exception("No se pudo cargar el id del predio");
            if (dr["dia_semana_id"] == null) throw new Exception("No se pudo cargar el id de un día de la semana");
            if (dr["hora"] == null) throw new Exception("No se pudo cargar el horario para el predio");
            #endregion

            #region Chequeo formato
            if (!Int32.TryParse(dr["predio_id"].ToString(), out int r_predio_id)) throw new Exception("El id del predio no tiene el formato correcto");
            if (!Int32.TryParse(dr["dia_semana_id"].ToString(), out int r_dia_semana)) throw new Exception("El dia de la semana no tiene el formato correcto");
            if (!Decimal.TryParse(dr["hora"].ToString(), out decimal r_hora)) throw new Exception("El horario de apertura no tiene el formato correcto");
            #endregion

            if (predioCache == null)
            {
                var predioResponse = Predio.Obtener(r_predio_id);
                if (predioResponse.Resultado == Tuneles.StatusCode.OK && predioResponse != null)
                    predioCache = predioResponse.Contenido;
                else throw new Exception("Hubo un error al obtener el predio del horario: " + predioResponse?.Mensaje);
            }
            return new HorarioPredio()
            {
                Predio = predioCache,
                DiaSemana = (DayOfWeek)r_dia_semana,
                Hora = r_hora
            };

        }
        #endregion
    }
}
