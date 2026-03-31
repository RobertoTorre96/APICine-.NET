using ApiCine.Features.Genero.DTOs;

namespace ApiCine.Features.Genero.Service {
    public interface IGeneroService {
        Task<IEnumerable<GeneroResponseDto>> FindAll();
        Task<GeneroResponseDto> Create(GeneroRequestDto generoRequestDto);

        Task<bool> Delete(long id);





    }
}
