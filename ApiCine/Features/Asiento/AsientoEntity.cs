using ApiCine.Features.Relaciones.ReservaAsientos;
using ApiCine.Features.Sala;

namespace ApiCine.Features.Asiento {
    public class AsientoEntity {
        public long Id { get; set; }

        public string Fila { get; set; } = null!;
        public int Numero { get; set; }
         public long SalaId { get; set; }
         public SalaEntity Sala { get; set; } = null!;

        public ICollection<ReservaAsientoEntity> reservaAsientos { get; set; } = null!;

    }
}
