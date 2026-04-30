using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Funcion.DTOs;
using ApiCine.Features.Pelicula;
using ApiCine.Features.Sala;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Funcion.Service {
    public class FuncionServiceImpl : IFuncionService {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FuncionServiceImpl(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }


        public async  Task<FuncionResponseDetalleDto> Crear(FuncionResquestDto request) {
            
            PeliculaEntity? pelicula = await _context.Pelicula.FindAsync(request.PeliculaId) 
                                      ?? throw new NotFoundException($"No se encontró la película con ID {request.PeliculaId}");

            SalaEntity sala = await _context.Sala.FindAsync(request.SalaId) 
                            ?? throw new NotFoundException($"No se encontró la sala con ID {request.SalaId}");

            DateTime inicioNueva = request.FechaHora;
            DateTime finNueva = inicioNueva.AddMinutes(pelicula.Duracion + 30);

            // 2. Separamos las condiciones para que sean legibles
            bool seSolapa = await _context.Funcion
                            .Include(f => f.Pelicula) // <--- AGREGAR ESTO
                            .AnyAsync(f =>
                            f.SalaId == request.SalaId && (
                                // ¿Mi INICIO cae dentro de una función existente?
                                (inicioNueva >= f.FechaHora && inicioNueva < f.FechaHora.AddMinutes(f.Pelicula.Duracion + 30)) ||

                                // ¿Mi FIN cae dentro de una función existente?
                                (finNueva > f.FechaHora && finNueva <= f.FechaHora.AddMinutes(f.Pelicula.Duracion + 30)) ||

                                // ¿Mi función es tan larga que "envuelve" a una existente?
                                (inicioNueva <= f.FechaHora && finNueva >= f.FechaHora.AddMinutes(f.Pelicula.Duracion + 30))
                            )
                         );

            if (seSolapa) { 
                throw new BadRequestException("La función se solapa con otra función existente en la misma sala.");
            }

            var entity= _mapper.Map<FuncionEntity>(request);
            _context.Funcion.Add(entity);
            await _context.SaveChangesAsync();

            var response =await _context.Funcion
                                .Include(Funcion => Funcion.Pelicula)
                                .Include(Funcion => Funcion.Sala)
                                .Include(f => f.ReservaAsientos)
                                .FirstOrDefaultAsync(f => f.Id == entity.Id);

            return _mapper.Map<FuncionResponseDetalleDto>(response);
        }

      

        public async Task<IEnumerable<FuncionResponseListaDto>> FindAll() {
            var funciones = await _context.Funcion
        .Include(f => f.Pelicula)
        .Include(f => f.Sala)
        .OrderBy(f => f.FechaHora)
        .ToListAsync();

            return _mapper.Map<IEnumerable<FuncionResponseListaDto>>(funciones);
        }
        public async Task<IEnumerable<FuncionResponseListaDto>> FindByPelicula(long peliculaId) {
            var funciones = await _context.Funcion
                            .Include(f => f.Pelicula)
                            .Include(f => f.Sala)
                            // En la lista general no solemos necesitar ReservaAsientos, 
                            // pero si tu DTO lo pide, déjalo.
                            .Where(f => f.PeliculaId == peliculaId && f.FechaHora >= DateTime.Now)
                            .OrderBy(f => f.FechaHora)
                            .ToListAsync(); // <-- Esto genera la lista

          
            // 3. Mapeamos la colección completa
            return _mapper.Map<IEnumerable<FuncionResponseListaDto>>(funciones);
        }

        public async Task<FuncionResponseDetalleDto> FindById(long id) {
        var response = await _context.Funcion
                                .Include(Funcion => Funcion.Pelicula)
                                .Include(Funcion => Funcion.Sala)
                                .Include(f => f.ReservaAsientos)
                                .FirstOrDefaultAsync(f => f.Id == id) 
                        ?? throw new NotFoundException($"No se encontró la función con ID {id}");
            return _mapper.Map<FuncionResponseDetalleDto>(response);


        }


        public async Task<bool> Delete(long id) {
            // Usamos FirstOrDefault con Include para cargar la lista de Reservas
            var entity = await _context.Funcion
                        .Include(f => f.Reservas)
                        .FirstOrDefaultAsync(f => f.Id == id)
                        ?? throw new NotFoundException($"No se encontró la función con ID {id}");

            // Ahora sí, .Any() detectará si hay reservas reales en la base de datos
            if (entity.Reservas != null && entity.Reservas.Any()) {
                throw new BadRequestException("No se puede eliminar una función que ya tiene reservas registradas.");
            }

            _context.Funcion.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
