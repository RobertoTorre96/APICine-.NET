namespace ApiCine.Features.Sala {
    public class SalaEntity {

        public long Id { get; set; }
        
        public ICollection<Funcion.FuncionEntity> Funciones { get; set; } = new HashSet<Funcion.FuncionEntity>();
    }
}
