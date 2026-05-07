namespace ApiCine.Features.Asiento.DTOs
{
    public class AsientoResponseDto
    {
        public long Id { get; set; }
        public string Fila { get; set; } = null!;
        public int Numero { get; set; }
    }
}
