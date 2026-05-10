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



        public async Task<ReservaResponseDto> Create(ReservaRequestDto request, long userId) {
            FuncionEntity funcion = await _context.Funcion
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.ReservaAsientos)
                    .ThenInclude(ra => ra.Reserva) 
                .FirstOrDefaultAsync(f => f.Id == request.FuncionId)
                ?? throw new NotFoundException($"Funcion con id {request.FuncionId} no encontrada");

            using var transaction = await _context.Database.BeginTransactionAsync();
                try {
                    var reserva = _mapper.Map<ReservaEntity>(request);
                    reserva.Cod = $"RES-{Guid.NewGuid().ToString()[..8].ToUpper()}";
                    reserva.Fecha = DateTime.Now;
                    reserva.UsuarioId = userId;


                _context.Reserva.Add(reserva);
                    await _context.SaveChangesAsync();

                    await ValidAndAddAsientos(reserva.Id, funcion,request.AsientosIds);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return await FindById(reserva.Id);
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    if (ex is BadRequestException || ex is NotFoundException) {
                        throw;
                    }
                    throw new Exception("Ocurrió un error inesperado al procesar su reserva.", ex);
            }

            }
        
        public async Task<ReservaResponseDto> FindById(long id ) {
            var reserva = await _context.Reserva
             .Include(r => r.Usuario)        
             .Include(r => r.Funcion)
                 .ThenInclude(f => f.Pelicula)
                     .ThenInclude(p => p.PeliculaGeneros)                           
             .Include(r => r.Funcion)
                 .ThenInclude(f => f.Sala)        
             .Include(r => r.ReservaAsientos)
                 .ThenInclude(ra => ra.Asiento)
             .FirstOrDefaultAsync(r => r.Id == id)
             ?? throw new NotFoundException($"La reserva con ID {id} no existe.");

            return _mapper.Map<ReservaResponseDto>(reserva);
        }

        public async Task<IEnumerable<ReservaResponseDto>> FindByUsuario(long usuarioId) {
            var reservas = await _context.Reserva
                           .Include(r => r.Funcion).ThenInclude(f => f.Pelicula)
                           .Include(r => r.Funcion).ThenInclude(f => f.Sala)
                           .Include(r => r.ReservaAsientos).ThenInclude(ra => ra.Asiento)
                           .Where(r => r.UsuarioId == usuarioId)
                           .OrderByDescending(r => r.Fecha) 
                           .ToListAsync();
            return _mapper.Map<IEnumerable<ReservaResponseDto>>(reservas);
        }



        public async Task<bool> Cancel(long id) {
            // 1. Buscamos la reserva
            var reserva = await _context.Reserva
                .Include(r => r.ReservaAsientos) 
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

        private async Task ValidAndAddAsientos(long reservaId, FuncionEntity funcion, List<long> asientosIds)
        {


            if (asientosIds == null || !asientosIds.Any())
            {
                throw new BadRequestException("Debe seleccionar al menos un asiento.");
            }

            if (asientosIds.Distinct().Count() != asientosIds.Count)
            {
                throw new BadRequestException("No puedes seleccionar el mismo asiento más de una vez.");
            }

            // 2. CARGA DE DATOS EN BLOQUE (Evita el problema N+1)

            // Traemos todos los detalles de los asientos solicitados en UNA sola consulta
            var asientosSolicitados = await _context.Asiento
                .Where(a => asientosIds.Contains(a.Id))
                .ToListAsync();

            if (asientosSolicitados.Count != asientosIds.Count)
            {
                throw new NotFoundException("Uno o más asientos seleccionados no existen.");
            }

            // Traemos todos los IDs de asientos ya ocupados para esta función en UNA sola consulta
            var idsAsientosOcupados = await _context.ReservaAsiento
                .Where(ra => ra.FuncionId == funcion.Id && ra.Reserva.Estado != EEstadoReserva.Cancelada)
                .Select(ra => ra.AsientoId)
                .ToListAsync();

            // 3. VALIDACIÓN DE CAPACIDAD (Usando los datos ya cargados)
            int capacidadTotal = funcion.Sala.CantidadFilas * funcion.Sala.CantidadColumnas;
            if (idsAsientosOcupados.Count + asientosSolicitados.Count > capacidadTotal)
            {
                throw new BadRequestException("Lo sentimos, no hay suficientes lugares disponibles.");
            }

            // 4. BUCLE DE PROCESAMIENTO (Ahora es ultra rápido porque no consulta la DB)
            foreach (var asiento in asientosSolicitados)
            {

                // Validar que el asiento pertenezca a la sala de la función
                if (asiento.SalaId != funcion.SalaId)
                {
                    throw new BadRequestException($"El asiento {asiento.Fila}{asiento.Numero} no pertenece a esta sala.");
                }

                // Validar si está ocupado comparando con la lista que trajimos antes
                if (idsAsientosOcupados.Contains(asiento.Id))
                {
                    throw new BadRequestException($"El asiento {asiento.Fila}{asiento.Numero} ya está reservado.");
                }

                // Si pasa todo, lo agregamos al contexto
                var reservaAsiento = new ReservaAsientoEntity
                {
                    ReservaId = reservaId,
                    AsientoId = asiento.Id,
                    FuncionId = funcion.Id
                };

                _context.ReservaAsiento.Add(reservaAsiento);


            }
        }

    }

}
