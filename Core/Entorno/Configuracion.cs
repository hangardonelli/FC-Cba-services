using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entorno
{

    public class Configuracion
    {
        /// <summary>
        /// Los formatos aceptados por FC - Cordoba para subir fotos (fotos de perfil, portada, logos, publicidades, etc)
        /// </summary>
        public static List<string> FormatosFotosPermitidos = new()
        {
            ".jpg",
            ".png",
            ".gif"
        };

        public static int PublicidadMinResolucion = 20;
        public static int PublicidadMaxResolucion = 2500;

    }
        /// <summary>
        /// En esta clase se almacenan las constantes de urls a apis, s3, etc, cadenas de conexión, etc
        /// </summary>
        public class Urls
    {
        /// <summary>
        /// La URL BASE donde se van a almacenar las fotos de perfil de los predios
        /// </summary>
        public static string FotoPerfil = "https://prueba.com/";

        /// <summary>
        /// La URL BASE donde se van a almacenar las fotos de portada de los predios
        /// </summary>
        public static string FotoPortada = "https://prueba2.com";

        /// <summary>
        /// La cadena de conexión de la base de datos relacional
        /// </summary>
        public static string ConexionSQL = "DATA";

        /// <summary>
        /// La URL BASE de la foto de los usuarios de la APP
        /// </summary>
        public static string FotoPortadaUsuario = "https://fotoPortada.com/";

    }
}
