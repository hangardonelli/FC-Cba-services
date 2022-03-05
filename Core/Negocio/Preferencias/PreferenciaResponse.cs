using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;
namespace Core.Negocio.Preferencias
{
    public class PreferenciaResponse :  Respuesta<Preferencia>
    {
        public AccionRealizada AccionRealizada { get; set ; }
    }

    public class PreferenciaListaResponse : Respuesta<List<Preferencia>>
    {
        public AccionRealizada AccionRealizada { get; set; }
    }


    public enum AccionRealizada
    {
        CONSULTAR = 0,
        AGREGAR = 1,
        ELIMINAR = 2,
        
    }
}
