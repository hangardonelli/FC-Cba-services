namespace API.Models
{
    public class Predio
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? email { get; set; }
        public string? direccion { get;set; }
        public List<int>? preferencias { get; set; }
        public int? localidad { get; set; }
        public string? fotoPerfil { get; set; }
        public string? fotoPortada { get; set; }
        public string? descripcion { get; set; }
        public bool? habilitado { get; set; }
        public List<Horario>? horario { get; set; }
        public int? idUsuarioPredio { get; set; }

    }
}
