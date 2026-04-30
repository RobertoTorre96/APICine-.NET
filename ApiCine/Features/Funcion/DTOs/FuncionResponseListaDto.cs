namespace ApiCine.Features.Funcion.DTOs {
    public class FuncionResponseListaDto {
        //¿Cuándo se usa? Cuando el usuario entra a la aplicación y ve la lista de todas las películas disponibles hoy.

        public long Id { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Precio { get; set; }

        // Solo lo necesario para identificar dónde y qué se proyecta
        public string PeliculaTitulo { get; set; } = string.Empty;
        public string SalaNombre { get; set; } = string.Empty;

        // Propiedades calculadas (=>) para que el Front no tenga que formatear fechas
        public string Hora => FechaHora.ToString("HH:mm");
        public string Fecha => FechaHora.ToString("dd/MM/yyyy");
    }
}
