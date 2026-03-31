using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Enums;
using ApiCine.Features.Funcion;
using ApiCine.Features.Relaciones.ReservaAsientos;
using ApiCine.Features.Reserva.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Reserva.Service {
    public class ReservaServiceImpl : IReservaService {


        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReservaServiceImpl(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }



        public async Task<ReservaResponseDto> RealizarReserva(ReservaRequestDto request) {
            FuncionEntity funcion = await _context.Funcion
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.ReservaAsientos)
                    .ThenInclude(ra => ra.Reserva) // <--- AGREGÁ ESTO
                .FirstOrDefaultAsync(f => f.Id == request.FuncionId)
                ?? throw new NotFoundException($"Funcion con id {request.FuncionId} no encontrada");

            using var transaction = await _context.Database.BeginTransactionAsync();
                try {
                    var reserva = _mapper.Map<ReservaEntity>(request);
                    reserva.Cod = $"RES-{Guid.NewGuid().ToString()[..8].ToUpper()}";
                    reserva.Fecha = DateTime.Now;

                    _context.Reserva.Add(reserva);
                    await _context.SaveChangesAsync();

                    await ValidarYAgregarAsientos(reserva.Id, funcion,request.AsientosIds);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return await GetById(reserva.Id);
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    if (ex is BadRequestException || ex is NotFoundException) {
                        throw;
                    }
                    throw new Exception("Ocurrió un error inesperado al procesar su reserva.", ex);
            }

            }
        
        public async Task<ReservaResponseDto> GetById(long id ) {
            var reserva = await _context.Reserva
             .Include(r => r.Usuario)        
             .Include(r => r.Funcion)
                 .ThenInclude(f => f.Pelicula)
                     .ThenInclude(p => p.PeliculaGeneros)                           
             .Include(r => r.Funcion)
                 .ThenInclude(f => f.Sala)        
             .Include(r => r.reservaAsientos)
                 .ThenInclude(ra => ra.Asiento)
             .FirstOrDefaultAsync(r => r.Id == id)
             ?? throw new NotFoundException($"La reserva con ID {id} no existe.");

            return _mapper.Map<ReservaResponseDto>(reserva);
        }

        public async Task<IEnumerable<ReservaResponseDto>> GetByUsuario(long usuarioId) {
            var reservas = await _context.Reserva
                           .Include(r => r.Funcion).ThenInclude(f => f.Pelicula)
                           .Include(r => r.Funcion).ThenInclude(f => f.Sala)
                           .Include(r => r.reservaAsientos).ThenInclude(ra => ra.Asiento)
                           .Where(r => r.UsuarioId == usuarioId)
                           .OrderByDescending(r => r.Fecha) 
                           .ToListAsync();
            return _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);
        }


        private async Task ValidarYAgregarAsientos(long reservaId, FuncionEntity funcion, List<long> asientosIds) {

            // 1. ESCUDOS INICIALES (Rápido, sin entrar al bucle)
            if (asientosIds.Distinct().Count() != asientosIds.Count) {
                throw new BadRequestException("No puedes seleccionar el mismo asiento más de una vez.");
            }

            int capacidadTotal = funcion.Sala.CantidadFilas * funcion.Sala.CantidadColumnas;

            // Contamos solo las reservas que NO están canceladas para ver la ocupación real
            int ocupadosReales = funcion.ReservaAsientos
                .Count(ra => ra.Reserva.Estado != EEstadoReserva.Cancelada);

            if (ocupadosReales + asientosIds.Count > capacidadTotal) {
                throw new BadRequestException("Lo sentimos, no hay suficientes lugares disponibles en esta función.");
            }

            // 2. BUCLE DE VALIDACIÓN Y CARGA
            foreach (var asientoId in asientosIds) {
                var asiento = await _context.Asiento.FindAsync(asientoId)
                    ?? throw new NotFoundException($"El asiento con ID {asientoId} no existe.");

                if (asiento.SalaId != funcion.SalaId) {
                    throw new BadRequestException($"El asiento {asiento.Fila}{asiento.Numero} no pertenece a esta sala.");
                }

                // VALIDACIÓN CLAVE: ¿Está ocupado por alguien que NO haya cancelado?
                bool ocupado = await _context.ReservaAsiento.AnyAsync(ra =>
                    ra.FuncionId == funcion.Id &&
                    ra.AsientoId == asientoId &&
                    ra.Reserva.Estado != EEstadoReserva.Cancelada); 

                if (ocupado) {
                    throw new BadRequestException($"El asiento {asiento.Fila}{asiento.Numero} ya está reservado.");
                }

                var reservaAsiento = new ReservaAsientoEntity {
                    ReservaId = reservaId,
                    AsientoId = asientoId,
                    FuncionId = funcion.Id
                };
                _context.ReservaAsiento.Add(reservaAsiento);
            }
        }

        public async Task<bool> CancelarReserva(long id) {
            // 1. Buscamos la reserva
            var reserva = await _context.Reserva
                .Include(r => r.reservaAsientos) 
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException($"La reserva con ID {id} no existe.");

            if (reserva.Estado == EEstadoReserva.Cancelada) {
                throw new BadRequestException("La reserva ya se encuentra cancelada.");
            }     
            
            var funcion = await _context.Funcion.FindAsync(reserva.FuncionId)
                        ?? throw new NotFoundException($"La función asociada a la reserva no existe."); 

            if (funcion.FechaHora < DateTime.Now.AddHours(1)) {
                throw new BadRequestException("No se pueden cancelar reservas con menos de 1 hora de antelación.");
            }       
            
            reserva.Estado = EEstadoReserva.Cancelada;
            await _context.SaveChangesAsync();

            return true;
        }
    }
        
}
