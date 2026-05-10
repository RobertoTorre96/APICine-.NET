using ApiCine.Features.Reserva.DTOs;

namespace ApiCine.Features.Reserva.Service {
    public interface IReservaService {
        Task<ReservaResponseDto> Create(ReservaRequestDto request,long userId);
        Task<ReservaResponseDto> FindById(long id);
        Task<IEnumerable<ReservaResponseDto>> FindByUsuario(long usuarioId);
        Task<bool> Cancel(long id);

    }
}
