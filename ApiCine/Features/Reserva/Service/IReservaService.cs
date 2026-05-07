using ApiCine.Features.Reserva.DTOs;

namespace ApiCine.Features.Reserva.Service {
    public interface IReservaService {
        Task<ReservaResponseDto> RealizarReserva(ReservaRequestDto request,long userId);
        Task<ReservaResponseDto> GetById(long id);
        Task<IEnumerable<ReservaResponseDto>> GetByUsuario(long usuarioId);
        Task<bool> CancelarReserva(long id);

    }
}
