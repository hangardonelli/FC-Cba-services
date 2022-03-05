using System.Xml.Serialization;
using System.Text;
using System.Net.Mail;

namespace Core
{
    public static class Extensiones
    {
        /// <summary>
        /// Convierte a MD5 el string actual
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5(this string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
        /// <summary>
        /// Valida un domiclio
        /// </summary>
        public static bool ValidarDireccion(this string direccion) => String.IsNullOrEmpty(direccion?.Trim());
        /// <summary>
        /// Indica si un mail tiene el formato correcto o no. En caso de no estarlo, explica por out string? mensaje el motivo por el cuál no es válido
        /// </summary>
        public static bool ValidarEmail(this string email, out string? mensaje)
        {
            try
            {
                if (email == null) throw new ArgumentNullException(nameof(email));
                MailAddress mailAddress = new(email);
                mensaje = null;
                return true;
            }
            catch (ArgumentNullException)
            {
                mensaje = "No se ha establecido un correo electrónico para la cuenta";
                return false;
            }
            catch (FormatException)
            {
                mensaje = "El correo electrónico no tiene el formato correcto";
                return false;
            }
            catch (Exception ex)
            {
                mensaje = "Se ha producido el error al intentar obtener el correo electrónico: " + ex.Message;
                return false;
            }

        }
        /// <summary>
        /// Indica si un string está compuesto solamente de digitos
        /// </summary>
        public static bool EsDigito(this string s)
        {
            if (s == null || s == "") return false;
            for (int i = 0; i < s.Length; i++)
                if ((s[i] ^ '0') > 9)
                    return false;

            return true;
        }
        /// <summary>
        /// Convierte a XML el la instancia del objeto actual
        /// </summary>
        public static string ToXML(this object objeto)
        {
            try
            {
                using (var stringwriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(objeto.GetType());
                    serializer.Serialize(stringwriter, objeto);
                    return stringwriter.ToString();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    return $"Error al convertir el objeto '{objeto.GetType().Name}' a XML -> " + ex.Message;
                }
                catch(Exception ex2)
                {
                    return $"Error al convertir el objeto ({ex.Message}) a XML -> " + ex.Message;
                }
            }
        }
    }
}
