using ApiCine.Features.Usuario.DTOs;

namespace ApiCine.Features.Usuario.Service {
    public interface IUsuarioService {
        Task<UsuarioResponseDto> Registrar(UsuarioRequestDto request);
        Task<UsuarioResponseDto> GetById(long id);
        Task<IEnumerable<UsuarioResponseDto>> GetAll();
    }
}
