namespace ApiCine.Features.Funcion.DTOs {
    public class FuncionResponseDetalleDto {
        //¿Cuándo se usa? Cuando el usuario ya hizo clic en una película específica
        //(ejemplo: "Avengers - 20:30hs") y pasa a la pantalla de selección de asientos.


        public long Id { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Precio { get; set; }

        // Información completa de la Película
        public long PeliculaId { get; set; }
        public string PeliculaTitulo { get; set; } = string.Empty;
        public string PeliculaSinopsis { get; set; } = string.Empty;
        public int PeliculaDuracion { get; set; }

        // Información de la Sala (incluyendo dimensiones para dibujar el mapa)
        public string SalaNombre { get; set; } = string.Empty;
        public int CantidadFilas { get; set; }
        public int CantidadColumnas { get; set; }

        // Aquí podrías incluir la lista de asientos ya ocupados
        public List<long> AsientosOcupadosIds { get; set; } = new();
    }
}
