namespace ApiCine.Features.Relaciones.ReservaAsientos {
    public class ReservaAsientoEntity {
        public long Id { get; set; }

        public long ReservaId { get; set; }
        public Reserva.ReservaEntity Reserva { get; set; } = null!;

        public long AsientoId { get; set; }
        public Asiento.AsientoEntity Asiento { get; set; } = null!;

        public long FuncionId { get; set; }
        public Funcion.FuncionEntity Funcion { get; set; } = null!;


    }
}
