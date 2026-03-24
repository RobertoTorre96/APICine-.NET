using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Genero.DTOs {
    public class GeneroRequestDto {
        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

    }


}
