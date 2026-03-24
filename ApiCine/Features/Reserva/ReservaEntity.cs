using ApiCine.Features.Funcion;
using ApiCine.Features.Relaciones.ReservaAsientos;
using ApiCine.Features.Usuario;

namespace ApiCine.Features.Reserva {
    public class ReservaEntity {

        public long Id { get; set; }
        public string Cod { get;set; }  = null!;
        public DateTime Fecha { get; set; }
        public long FuncionId { get; set; }
        public FuncionEntity Funcion { get; set; } = null!;
        public ICollection<ReservaAsientoEntity> reservaAsientos { get; set; }= new List<ReservaAsientoEntity>();

        public long UsuarioId { get; set; }
        public UsuarioEntity Usuario { get; set; } = null!;

    }
}
