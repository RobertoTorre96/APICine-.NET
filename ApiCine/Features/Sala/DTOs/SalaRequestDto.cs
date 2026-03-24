using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Sala.DTOs {
    public class SalaRequestDto {
        [Required(ErrorMessage = "El código de la sala es obligatorio.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "El código debe tener entre 2 y 10 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes indicar cuántas filas tiene la sala.")]
        [Range(1, 30, ErrorMessage = "Las filas deben estar entre 1 y 30.")]
        public int CantidadFilas { get; set; }

        [Required(ErrorMessage = "Debes indicar cuántas columnas tiene la sala.")]
        [Range(1, 30, ErrorMessage = "Las columnas deben estar entre 1 y 30.")]
        public int CantidadColumnas { get; set; }

    }
}
