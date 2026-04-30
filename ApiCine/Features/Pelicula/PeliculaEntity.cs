using ApiCine.Features.Relaciones.PeliculaGenero;

namespace ApiCine.Features.Pelicula {
    public class PeliculaEntity {

        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Titulo { get; set; }=null!;
        public int Duracion { get; set; }
        public string Sinopsis { get; set; } = string.Empty;

        public ICollection<PeliculaGeneroEntity> PeliculaGeneros { get; set; } = new HashSet<PeliculaGeneroEntity>();
        public ICollection<Funcion.FuncionEntity> Funciones { get; set; } = new HashSet<Funcion.FuncionEntity>();
    }
}
