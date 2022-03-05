using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;
namespace Core.Negocio.HorariosPredios
{
    public class HorarioPredioResponse : Respuesta<HorarioPredio>
    {
        public AccionRealizada AccionRealizada { get; set; }
    }

    public class HorarioPredioListaResponse : Respuesta<List<HorarioPredio>>
    {
        public AccionRealizada AccionRealizada { get; set; }
    }

    public enum AccionRealizada
    {
        CONSULTAR = 0,
        MODIFICAR = 1,
        CREAR = 2,
        ELIMINAR = 3
    }
}
