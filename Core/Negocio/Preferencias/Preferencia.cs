using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Negocio.Predios;
namespace Core.Negocio.Preferencias
{
    public class Preferencias
    {
        #region Metodos Publicos


        public static PreferenciaListaResponse Obtener(Predio predio)
        {
            PreferenciaListaResponse response = new PreferenciaListaResponse() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                #region Chequeo nulls
                if (!predio.Id.HasValue) throw new Exception("No se ha inicializado el identificador del predio");
                #endregion

                DataTable datos  = Datos.Preferencias.Preferencias.ObtenerPreferencia(predio.Id.Value);

                var preferenciasResponse = Obtener(datos);

                if (preferenciasResponse.Resultado != Tuneles.StatusCode.OK) throw new Exception(preferenciasResponse.Mensaje);


                response.Contenido = preferenciasResponse.Contenido;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un consultar las preferencia del predio: " + ex.Message;
            }

            return response;
        }

        public static PreferenciaResponse Crear(Predio predio, Preferencia preferencia)
        {
            PreferenciaResponse response = new PreferenciaResponse() { AccionRealizada = AccionRealizada.AGREGAR };
            try
            {
                #region Chequeo nulls
                if (!predio.Id.HasValue) throw new Exception("No se ha inicializado el identificador del predio");
                #endregion

                _ = Datos.Preferencias.Preferencias.Crear(predio.Id.Value, (int)preferencia);
                response.Contenido = preferencia;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un error al crear la preferencia: " + ex.Message;
            }

            return response;
        }

        public static PreferenciaResponse Eliminar(Predio predio, Preferencia preferencia)
        {
            PreferenciaResponse response = new PreferenciaResponse() { AccionRealizada = AccionRealizada.ELIMINAR };
            try
            {
                #region Chequeo nulls
                if (!predio.Id.HasValue) throw new Exception("No se ha inicializado el identificador del predio");
                #endregion

                _ = Datos.Preferencias.Preferencias.Eliminar(predio.Id.Value, (int)preferencia);
                response.Contenido = preferencia;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un error al eliminar la preferencia: " + ex.Message;
            }

            return response;
        }

        public static PreferenciaListaResponse Obtener(DataTable dt)
        {
            PreferenciaListaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                List<Preferencia> preferencias = new();

                foreach (DataRow dr in dt.Rows)
                {
                    var responsePreferencia = Obtener(dr);
                    if(responsePreferencia.Resultado == Tuneles.StatusCode.OK)
                        preferencias.Add(responsePreferencia.Contenido);
                }

                response.Contenido = preferencias;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un error la obtener una preferencia: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Obtiene una preferencia a partir de un datarow
        /// </summary>
        public static PreferenciaResponse Obtener(DataRow dr)
        {
            PreferenciaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };

            try
            {
                #region Chequeo nulls
                if (dr["preferencia_id"] == null) throw new Exception("No se ha cargado el identificador de la preferencia");
                #endregion

                #region Chequeo formato
                if (!Int32.TryParse(dr["preferencia_id"].ToString(), out int r_preferencia_id)) throw new Exception("El identificador de la preferencia no tiene el formato correcto");
                #endregion

                response.Contenido = (Preferencia)r_preferencia_id;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se produjo un error la obtener una preferencia: " + ex.Message;
            }

            return response;
        }
        #endregion


    }
    public enum Preferencia
    {
        Techada = 0,
        Parrilla = 1,
        Buffet = 2,
        Vestuario = 3
    }
}
