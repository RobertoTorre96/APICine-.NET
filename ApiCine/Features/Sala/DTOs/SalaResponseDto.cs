namespace ApiCine.Features.Sala.DTOs {
    public class SalaResponseDto {
        public long Id { get; set; }
        public string Cod { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int CantidadFilas { get; set; }
        public int CantidadColumnas { get; set; }
        public int CapacidadTotal => CantidadFilas * CantidadColumnas;
    }
}
