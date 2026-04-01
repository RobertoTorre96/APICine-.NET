using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Usuario.DTOs {
    public class UsuarioLoginRequestDto {
        [Required(ErrorMessage = "El campo 'Email' es obrigatório.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "El campo 'Password' es obrigatório.")]
        public string Password { get; set; } = null!;
    }
}
