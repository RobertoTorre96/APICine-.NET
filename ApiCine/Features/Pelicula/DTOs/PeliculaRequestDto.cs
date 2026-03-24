using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Pelicula.DTOs {
    public class PeliculaRequestDto {

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El título debe tener entre 2 y 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, 600, ErrorMessage = "La duración debe ser entre 1 y 600 minutos.")]
        public int Duracion { get; set; }

        [Required(ErrorMessage = "La sinopsis es obligatoria.")]
        public string Sinopsis { get; set; } = string.Empty;

        // Recibimos solo los IDs de los géneros que queremos asociar
        public List<int> GenerosIds { get; set; } = new();
    }
}
