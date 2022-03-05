using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Entorno;
namespace Core.Negocio.Publicidades
{
    public class Publicidad
    {
        #region Propiedades públicas

        /// <summary>
        /// El identificador de la publicidad
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// El nombre de la publicidad, ejemplo: Foto Banner iFK Sport
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// El nombre del archivo en el medio de almacenamiento seleccionado
        /// </summary>
        public string? Archivo { get; set; }
        /// <summary>
        /// El tipo de publicidad, Standard y Premium. 
        /// </summary>
        public TiposPublicidad? Tipo { get; set; }

        /// <summary>
        /// La resolución de la imagen de la publicidad expresada en pixeles AnchoXAlto (ej: 800x600)
        /// </summary>
        public string? Resolucion { get; set; }
        public DateTime? FechaAlta { get; set; }
        /// <summary>
        /// Fecha de caducidad de la publicidad. Pasada esta fecha aunque se encuentre en la base de datos, no se va a mostrar. NULL = Sin caducidad
        /// </summary>
        public DateTime? FechaExpiracion { get; set; }
        #endregion


        #region Metodos públicos

        public PublicidadResponse Modificar()
        {
            PublicidadResponse response = new PublicidadResponse() { AccionRealizada = AccionRealizada.MODIFICAR };
            try
            {
                #region Chequeo nulls
                if (!Id.HasValue) throw new Exception("La publicidad no se encuentra inicializada");
                if (Nombre == null) throw new Exception("No se ha definido un nombre para la publicidad");
                if (Archivo == null) throw new Exception("No se ha definido el nombre del archivo de esta publicidad");
                if (!Tipo.HasValue) throw new Exception("No se ha definido el tipo de publicidad");
                if (Resolucion == null) throw new Exception("No se ha definido la resolución de la publicidad");
                #endregion


                #region Validacion datos
                if (Nombre.Length <= 3) throw new Exception($"El nombre elegido ({Nombre}) es muy corto. Por favor elegí uno mas largo (más de 4 carácteres)");
                if (Nombre.Length >= 35) throw new Exception($"El nombre elegido ({Nombre}) es muy largo. Por favor elegí uno mas corto (hasta 35 carácteres)");
                #endregion

                #region Validación resolución
                if (!ValidarResolucion(out string? mensaje)) throw new Exception(mensaje);
                #endregion

                _ = Datos.Publicidades.Publicidad.Modificar(Id.Value, Nombre, Archivo, (int)Tipo.Value, Resolucion, FechaExpiracion);

                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public PublicidadResponse Crear()
        {
            PublicidadResponse response = new PublicidadResponse() { AccionRealizada = AccionRealizada.CREAR };

            try
            {
                #region Chequeo NULLS
                if (Resolucion == null) throw new Exception("No se estableció la resolución de la imagen");
                if (Nombre == null) throw new Exception("No se estableció el nombre de la publicidad");
                if (Archivo == null) throw new Exception("No se estableció el nombre del archivo de la publicidad");
                if (Tipo == null) throw new Exception("No se establecio el tipo de publicidad");
                if (!ValidarResolucion(out string? mensaje)) throw new Exception(mensaje);
                #endregion

                DataTable datos  = Datos.Publicidades.Publicidad.Crear(Nombre, Archivo, (int)Tipo, Resolucion, FechaExpiracion);

                if (!Int32.TryParse(datos.Rows[0]["id"].ToString(), out int r_id)) throw new Exception("Hubo un error al insertar en la base de datos la publicidad");

                Id = r_id;
                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Contenido = null;
                response.Mensaje = "Se produjo un error al crear la publicidad: " + ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Elimina de la base de datos la instancia de la publicidad actual
        /// </summary>
        public PublicidadResponse Eliminar()
        {
            PublicidadResponse response = new PublicidadResponse() { AccionRealizada = AccionRealizada.ELIMINAR };
            try
            {
                if (!Id.HasValue) throw new Exception("La publicidad no se encuentra inicializada");
                _ = Datos.Publicidades.Publicidad.EliminarPublicidad(Id.Value);

                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Obtiene una lista con todas las publicidades
        /// </summary>
        public static PublicidadListaResponse ObtenerTodas()
        {
            PublicidadListaResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                List<Publicidad> publicidades = new();

                DataTable datos = Datos.Publicidades.Publicidad.ObtenerTodas();

                foreach (DataRow dr in datos.Rows)
                {
                    var publicidadResponse = Obtener(dr);
                    if (publicidadResponse.Resultado != Tuneles.StatusCode.OK || publicidadResponse.Contenido == null) throw new Exception(publicidadResponse.Mensaje);
                    publicidades.Add(publicidadResponse.Contenido);
                }

                response.Contenido = publicidades;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Obtiene una publicidad por su id
        /// </summary>
        public static PublicidadResponse Obtener(int id)
        {

            PublicidadResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                Publicidad? publicidad = new();

                DataTable datos = Datos.Publicidades.Publicidad.Obtener(id);

                var publicidadDtResponse = Obtener(datos);

                if (publicidadDtResponse.Resultado != Tuneles.StatusCode.OK) throw new Exception(publicidadDtResponse.Mensaje);
                response.Contenido = publicidadDtResponse.Contenido;
                response.Resultado = Tuneles.StatusCode.OK;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }


        /// <summary>
        /// Obtiene una publicidad a partir de un DataTable
        /// </summary>
        public static PublicidadResponse Obtener(DataTable dt)
        {
            PublicidadResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                Publicidad? publicidad = new();

                if (dt.Rows.Count == 0) publicidad = null;
                foreach (DataRow dr in dt.Rows)
                {
                    var publicidadResponse = Obtener(dr);
                    if (publicidadResponse.Resultado == Tuneles.StatusCode.OK)
                        publicidad = publicidadResponse.Contenido;
                    break;
                }

                if (publicidad == null) throw new Exception("No se encontró la publicidad solicitada");
                response.Contenido = publicidad;
                response.Resultado = Tuneles.StatusCode.OK;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Obtiene una publicidad a partir de un datarow
        /// </summary>
        public static PublicidadResponse Obtener(DataRow dr)
        {
            PublicidadResponse response = new() { AccionRealizada = AccionRealizada.CONSULTAR };
            try
            {
                #region Chequeo nulls
                if (dr["id"] == null) throw new NullReferenceException("No se ha inicializado el id");
                if (dr["nombre"] == null) throw new NullReferenceException("No se pudo obtener el nombre de la publicidad");
                if (dr["archivo"] == null) throw new NullReferenceException("No se pudo obtener el archivo asociado a la publicidad");
                if (dr["tipo"] == null) throw new NullReferenceException("No se pudo obtener el tipo de publicidad");
                if (dr["resolucion"] == null) throw new NullReferenceException("No se pudo establecer la resolución de la publicidad");
                if (dr["fecha_alta"] == null) throw new NullReferenceException("No se pudo obtener la fecha de alta de la publicidad");
                if (dr["fecha_expiracion"] == null) throw new NullReferenceException("No se ha podido obtener la fecha de expiración de la publicidad");
                #endregion

                #region Validaciones formato
                if (!Int32.TryParse(dr["id"].ToString(), out int r_id)) throw new Exception("El identificador de la publicidad no tiene el formato correcto");
                if (dr["nombre"] == DBNull.Value) throw new Exception("No se cargó el nombre de la publicidad");
                if (!Int32.TryParse(dr["tipo"].ToString(), out int r_tipo)) throw new Exception("El tipo de publicidad no tiene el formato correcto");
                if (dr["fecha_alta"] == DBNull.Value) throw new Exception("No se ha podido establecer la fecha de carga de la publicidad");
                if (dr["archivo"] == DBNull.Value) throw new Exception("No se ha podido establecer el nombre del archivo de  la publicidad");
                if (dr["resolucion"] == DBNull.Value) throw new Exception("No se ha podido establecer el tamaño del archivo de  la publicidad");
                #endregion
                Publicidad publicidad = new();
                publicidad.Id = r_id;
                publicidad.Archivo = dr["archivo"].ToString();
                publicidad.Nombre = dr["nombre"].ToString();
                publicidad.Tipo = (TiposPublicidad)r_tipo;
                publicidad.Resolucion = dr["resolucion"].ToString();
                publicidad.FechaAlta = (DateTime)dr["fecha_alta"];
                publicidad.FechaExpiracion = dr["fecha_expiracion"] == DBNull.Value ? null : (DateTime)dr["fecha_expiracion"];


                response.Contenido = publicidad;
                response.Resultado = Tuneles.StatusCode.OK;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Hubo un error al obtener la publicidad: " + ex.Message;
            }

            return response;
        }
        #endregion

        #region Metodos privados

        /// <summary>
        /// Valida la resolución de la foto de la publicidad
        /// </summary>
        private bool ValidarResolucion(out string? mensaje)
        {
            try
            {
                if (Archivo == null) throw new Exception("No se ha establecido el nombre del archivo de la foto de la publicidad");

                string? formatoFoto = null;
                
                //Verificar formato de la foto:
                foreach (string formato in Configuracion.FormatosFotosPermitidos)
                {
                    if (Archivo.EndsWith(formato))
                    {
                        formatoFoto = formato;
                        break;
                    }
                }
                if (formatoFoto == null) throw new Exception("El formato de la foto no es válido. Solo se aceptan los formatos " + String.Join(", ", Configuracion.FormatosFotosPermitidos));
             
                if (Resolucion == null) throw new Exception("No se ha establecido la resolución de la foto de la publicidad");
                string[] resolucionArr = Resolucion.Split('x');

                //Existencia de anchura y altura:
                if (resolucionArr.Length != 2) throw new Exception("La resolución no tiene el formato correcto. Por favor introduca correctamente la resolucion");

                //Formato:
                if (!Int32.TryParse(resolucionArr[0], out int r_anchura)) throw new Exception("El ancho de la foto no tiene el formato correcto");
                if (!Int32.TryParse(resolucionArr[1], out int r_altura)) throw new Exception("El alto de la foto no tiene el formato correcto");

                //Validación tamaño minimo permitido
                if (r_anchura < Configuracion.PublicidadMinResolucion) throw new Exception($"La anchura de la foto es muy pequeña. Por favor utilice una anchura superior a {Configuracion.PublicidadMinResolucion} pixeles");
                if (r_altura < Configuracion.PublicidadMinResolucion) throw new Exception($"La altura de la foto es muy pequeña. Por favor utilice una altura superior a {Configuracion.PublicidadMinResolucion} pixeles");

                //Validación tamaño máximo permitdo:
                if (r_anchura > Configuracion.PublicidadMaxResolucion) throw new Exception($"La anchura de la foto es muy grande. Por favor utilice una anchura menor a {Configuracion.PublicidadMaxResolucion} pixeles");
                if (r_altura > Configuracion.PublicidadMaxResolucion) throw new Exception($"La altura de la foto es muy grande. Por favor utilice una anchura menor a {Configuracion.PublicidadMaxResolucion} pixeles");

                mensaje = null;
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }

        }
        #endregion
    }



}
