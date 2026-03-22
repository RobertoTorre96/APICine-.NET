using ApiCine.Features.Genero;
using ApiCine.Features.Pelicula;

namespace ApiCine.Features.Relaciones.PeliculaGenero {
    public class PeliculaGeneroEntity {

        public long Id { get; set; }
        public long PeliculaId { get; set; }
        public PeliculaEntity Pelicula { get; set; } = null!;

        public long GeneroId { get; set; }
        public GeneroEntity Genero { get; set; }= null!;
    }
}
