using ApiCine.Features.Pelicula;
using ApiCine.Features.Sala;

namespace ApiCine.Features.Funcion {
    public class FuncionEntity {

        public long Id { get; set; }

        public decimal Precio { get; set; }
        public DateTime FechaHora { get; set; }

        public long PeliculaId { get; set; }
        public PeliculaEntity Pelicula { get; set; } = null!;

        public long SalaId { get; set; }
        public SalaEntity Sala { get; set; } = null!;


    }
}
