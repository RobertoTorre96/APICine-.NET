using ApiCine.Features.Asiento;
using ApiCine.Features.Funcion;

namespace ApiCine.Features.Sala {
    public class SalaEntity {

        public long Id { get; set; }

        public string Cod { get; set; } = null!;
        public string Nombre { get; set; } = null!;

        public int CantidadFilas { get; set; }

        public int CantidadColumnas { get; set; }
        public ICollection<AsientoEntity> Asientos { get; set; } = new HashSet<AsientoEntity>();

        public ICollection<FuncionEntity> Funciones { get; set; } = new HashSet<FuncionEntity>();
    }
}
