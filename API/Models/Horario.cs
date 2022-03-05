namespace API.Models
{
    public class Horario
    {
        #region Propiedades públicas
        public int? idPredio { get; set; }
        public decimal? hora { get; set; }
        public int? diaSemana { get; set; }
        #endregion

        public Horario(int? _idpredio, decimal? _hora, int? _diaSemana)
        {
            idPredio = _idpredio;
            hora = _hora;
            diaSemana = _diaSemana;
        }
    }


}
