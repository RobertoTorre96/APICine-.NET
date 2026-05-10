using ApiCine.Features.Usuario.DTOs;

namespace ApiCine.Features.Usuario.Service {
    public interface IUsuarioService {
        Task<UsuarioResponseDto> Create(UsuarioRequestDto request);
        Task<UsuarioResponseDto> FindById(long id);
        Task<IEnumerable<UsuarioResponseDto>> FindAll();
    }
}
