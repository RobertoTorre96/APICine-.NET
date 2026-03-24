using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Funcion.DTOs {
    public class FuncionResquestDto {

        [Required(ErrorMessage = "Debes seleccionar una película.")]
        public long PeliculaId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una sala.")]
        public long SalaId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        // Validación: No podrías crear funciones en el pasado (esto se suele hacer en el Service)
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }
    }
}
