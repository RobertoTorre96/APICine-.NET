using ApiCine.common.DTOs;
using ApiCine.Features.Pelicula.DTOs;

namespace ApiCine.Features.Pelicula.Service {
    public interface IPeliculaService {

        Task<PeliculaResponseDto> Create(PeliculaRequestDto request);
        Task<PageResponseDto<PeliculaResponseDto>> FindAll(int numeroPagina, int tamPagina);
        Task<PeliculaResponseDto> FindById(long id);
        
    }
}
