namespace ApiCine.Features.Pelicula.DTOs {
    public class PeliculaResponseDto {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public string Sinopsis { get; set; } = string.Empty;

        // En lugar de devolver la tabla intermedia, devolvemos los nombres: ["Acción", "Terror"]
        public List<string> Generos { get; set; } = new();

        // Propiedad calculada (flechita =>) para mostrar la duración linda
        public string DuracionTexto => $"{Duracion / 60}h {Duracion % 60}m";
    }
}
