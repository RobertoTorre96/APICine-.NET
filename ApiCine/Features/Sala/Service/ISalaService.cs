using ApiCine.Features.Sala.DTOs;

namespace ApiCine.Features.Sala.Service {
    public interface ISalaService {

        Task<SalaResponseDto> Create(SalaRequestDto request);
        Task<IEnumerable<SalaResponseDto>> FindAll();
        Task<SalaResponseDto> FindById(long id);        
        Task<SalaResponseDto> FindByCod(string cod);
        Task<bool> Delete(long id);
    }
}
