using ApiCine.Features.Genero;

namespace ApiCine.Features.Relaciones.PeliculaGenero {
    public class PeliculaGeneroEntity {

        public long PeliculaId { get; set; }
        public PeliculaEntity Pelicula { get; set; }

        public long GeneroId { get; set; }
        public GeneroEntity Genero { get; set; }
    }
}
