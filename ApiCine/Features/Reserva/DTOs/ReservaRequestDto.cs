using System.ComponentModel.DataAnnotations;

namespace ApiCine.Features.Reserva.DTOs {
    public class ReservaRequestDto {
        [Required(ErrorMessage = "La función es obligatoria.")]
        public long FuncionId { get; set; }


        [Required(ErrorMessage = "Debes seleccionar al menos un asiento.")]
        [MinLength(1, ErrorMessage = "La reserva debe tener al menos un asiento.")]
        public List<long> AsientosIds { get; set; } = new();
    }
}
