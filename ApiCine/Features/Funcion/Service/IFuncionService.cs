using ApiCine.Features.Asiento;
using ApiCine.Features.Asiento.DTOs;
using ApiCine.Features.Funcion.DTOs;

namespace ApiCine.Features.Funcion.Service {
    public interface IFuncionService {
        Task<FuncionResponseDetalleDto> Crear(FuncionResquestDto request);

        Task<IEnumerable<FuncionResponseListaDto>> FindAll();

        Task<IEnumerable<FuncionResponseListaDto>> FindByPelicula(long peliculaId);

        Task<FuncionResponseDetalleDto> FindById(long id);

        Task<bool> Delete(long id);
        Task<List<AsientoResponseDto>> GetAsientosDisponibles(long funcionId)
    }
}
