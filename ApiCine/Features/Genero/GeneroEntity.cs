using ApiCine.Features.Relaciones.PeliculaGenero;

namespace ApiCine.Features.Genero {
    public class GeneroEntity {

        public long Id { get; set; }

        public string Nombre { get; set; }=string.Empty;

        public ICollection<PeliculaGeneroEntity> PeliculaGeneros { get; set; } = new HashSet<PeliculaGeneroEntity>();


        
        
        
       
    }
}
