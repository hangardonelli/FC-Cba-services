using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Negocio
{


    public enum TipoCesped
    {
        Natural = 1,
        Sintetico = 2
    }

    public enum ProvinciasArgentina
    {
        BuenosAires,
        CapitalFederal,
        Catamarca,
        Chaco,
        Chubut,
        Cordoba,
        Corrientes,
        EntreRios,
        Formosa,
        Jujuy,
        LaPampa,
        LaRioja,
        Mendoza,
        Misiones,
        Neuquen,
        RioNegro,
        Salta,
        SanJuan,
        SanLuis,
        SantaCruz,
        SantaFe,
        SantiagodelEstero,
        TierradelFuego,
        Tucuman
    }
    public enum EstadoTurno
    {
        /// <summary>
        /// El turno está habilitado y se encuentra en espera para su posterior finalización
        /// </summary>
        Alquilado = 0,
        /// <summary>
        /// El turno fue finalizado correctamente
        /// </summary>
        Finalizado = 1,
        /// <summary>
        /// El cliente(usuario de la app) canceló el turno
        /// </summary>
        CanceladoPorCliente = 2,
        /// <summary>
        /// El predio canceló el turno
        /// </summary>
        CanceladoPorPredio = 3,


    }
    /// <summary>
    /// Indica los metodos de pago 
    /// </summary>
    public enum TiposPagoTurno
    {
        Efectivo,
        MercadoPagoParcial,
        MercadoPagoTotal
    }

    /// <summary>
    /// Denota los tipos de publicidad (premium aparece en lugares más llamativos que standard)
    /// </summary>
    public enum TiposPublicidad
    {
        /// <summary>
        /// Aparece en lugares secundarios
        /// </summary>
        Standard = 1,
        /// <summary>
        /// Aparece en los lugares mas frecuentados por los usuarios en la aplicación
        /// </summary>
        Premium = 2
    }
    /// <summary>
    /// La calificación que da el usuario basado en su experiencia con el turno
    /// </summary>
    public enum Calificaciones
    {
        MuyMala = 1,
        Mala = 2,
        Aceptable = 3,
        MuyBuena = 4,
        Excelente = 5
    }

    /// <summary>
    /// Indica la posición de fútbol
    /// </summary>
    public enum PosicionFutbol
    {
        Indefinido = 0,
        Arquero = 1,
        DefensaLateral = 2,
        DefensaCentral = 3,
        MediocampoLateral = 4,
        MediocampoCentral = 5,
        Enganche = 6,
        Delantero = 7
    }


}
