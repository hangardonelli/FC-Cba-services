using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;
namespace Core.Negocio.Publicidades
{
    public class PublicidadResponse : Respuesta<Publicidad>
    {
        public AccionRealizada AccionRealizada { get; set; }
    }

    public class PublicidadListaResponse : Respuesta<List<Publicidad>>
    {
        public AccionRealizada AccionRealizada { get; set; }

    }


    public enum AccionRealizada
    {
        CONSULTAR = 0,
        CREAR = 1,
        MODIFICAR = 2,
        ELIMINAR = 3
    }
}
