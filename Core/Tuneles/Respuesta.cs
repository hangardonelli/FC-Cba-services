using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tuneles
{
    public class Respuesta<T>
    {
        /// <summary>
        /// El estado de la respuesta
        /// </summary>
        public StatusCode Resultado { get; set; }
        /// <summary>
        /// Mensaje de la respuesta
        /// </summary>
        public string? Mensaje { get; set; }
        /// <summary>
        /// Cuando corresponda, el objeto asociado al tipo de respuesta
        /// </summary>
        public T? Contenido { get; set; }
        /// <summary>
        /// Campo adicional para utilizar como Tag cuando se lo requiera
        /// </summary>
        public object? Adicional { get; set; }
        /// <summary>
        /// Obtiene la instancia del objeto actual en XML
        /// </summary>
        public string XML { get => this.ToXML(); }

        public string? CodigoError { get; set; }
    }

    public enum StatusCode
    {
        /// <summary>
        /// La respuesta fue exitosa
        /// </summary>
        OK = 0,
        /// <summary>
        /// La respuesta se completó, pero hubo inconvenientes
        /// </summary>
        WARNING = 1,
        /// <summary>
        /// Hubo un error en la respuesta
        /// </summary>
        ERROR = 2
    }
    public enum OrigenError
    {
        /// <summary>
        /// Se produce cuando el origen del error se produce en la capa Datos
        /// </summary>
        Datos = 0,
        /// <summary>
        ///  Se produce cuando el origen del error se produce en la capa Negocio
        /// </summary>
        Negocio = 1
    }
}
