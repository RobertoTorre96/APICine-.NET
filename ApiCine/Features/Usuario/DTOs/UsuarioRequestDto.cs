using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Usuario.DTOs {
    public class UsuarioRequestDto {

        [Required(ErrorMessage ="el nombre de username no puede estar vacio")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }=string.Empty;

        [Required(ErrorMessage = "el nombre de nombre no puede estar vacio")]
        public string Nombre { get; set; }= string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Password { get; set; }= string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }= string.Empty;

        /// <summary>
        /// Rol del usuario: 1 = Cliente, 2 = Admin
        /// </summary>
        [Required(ErrorMessage = "El rol es obligatorio")]
        public int  Role { get; set; }


    }
}
