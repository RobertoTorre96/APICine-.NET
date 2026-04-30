using ApiCine.Features.Role;

namespace ApiCine.Features.Usuario {
    public class UsuarioEntity {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Nombre { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ERole Role { get; set; }

        public ICollection<Reserva.ReservaEntity> Reservas { get; set; } = new HashSet<Reserva.ReservaEntity>();
    }
}
