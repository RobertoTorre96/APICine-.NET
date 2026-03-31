namespace ApiCine.Features.Reserva.DTOs {
    public class ReservaResponseDto {
        public long Id { get; set; }
        public string Cod { get; set; } = string.Empty; // Ej: "RES-XJ82"
        public string Estado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

        // Datos de la película y sala para el ticket
        public string PeliculaTitulo { get; set; } = string.Empty;
        public string SalaNombre { get; set; } = string.Empty;
        public DateTime FechaHoraFuncion { get; set; }

        // En lugar de la tabla intermedia, devolvemos una lista amigable
        // Ejemplo: ["Fila A - Asiento 5", "Fila A - Asiento 6"]
        public List<string> AsientosReservados { get; set; } = new();

        public decimal PrecioTotal { get; set; }
    }
}
