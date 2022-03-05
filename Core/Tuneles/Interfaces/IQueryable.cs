using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tuneles;

namespace Core.Negocio.Interfaces
{
    public interface IConsultable<T>
    {
        T Agregar();
        T Modificar();
        T Eliminar();

    }
}
