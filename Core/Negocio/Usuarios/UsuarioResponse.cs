using Core.Tuneles;

namespace Core.Negocio.Usuarios
{
    public  class UsuarioResponse : Respuesta<Usuario>
    {
        public AccionRealizada AccionRealizada { get; set; }
    }
}
