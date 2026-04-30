namespace ApiCine.common.DTOs {
    public class PageResponseDto<T> {

        public ICollection<T> Items { get; set; }= new List<T>();
        public int numeroPagina { get; set; }
        public int TamPagina { get; set; }
        public int  TotalPaginas { get; set; }
        public int TotalItems { get; set; }

    }
}
