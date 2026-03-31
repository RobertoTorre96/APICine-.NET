using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Asiento;
using ApiCine.Features.Sala.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Sala.Service {
    public class SalaServiceImpl : ISalaService{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SalaServiceImpl(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SalaResponseDto> Crear(SalaRequestDto request) {

            string codRequet = request.Nombre.Trim().ToUpper();

            var existe= await _context.Sala.AnyAsync(s=>s.Cod==codRequet);
            if(existe)throw new AlreadyExistsException($"la Sala {request.Nombre} ya existe");

            var entity= _mapper.Map<SalaEntity>(request);

            for (int i = 0; i < request.CantidadFilas; i++) {
                for (int columna = 1; columna <= request.CantidadColumnas; columna++) {

                    AsientoEntity asiento= new AsientoEntity {
                                                        Fila = ((char)('A'+i)).ToString(),
                                                        Numero = columna,
                                                        Sala = entity
                    };
                    entity.Asientos.Add(asiento);
                }    
            }
            await _context.Sala.AddAsync(entity);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<SalaResponseDto>(entity);
            return response;


        }

        public async Task<bool> Delete(long id) {
        
            var entity = await _context.Sala.FindAsync(id);
            if (entity == null) throw new NotFoundException($"No se encontró la Sala con id {id}");
           
            bool tieneFuciones = await _context.Funcion.AnyAsync(f => f.SalaId == id);
            if (tieneFuciones) throw new BadRequestException($"No se puede eliminar la Sala con id {id} porque tiene funciones asociadas");

            _context.Sala.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SalaResponseDto>> FindAll() {
            
            var entities = await _context.Sala.ToListAsync();

            var response = _mapper.Map<IEnumerable<SalaResponseDto>>(entities);

            return response;
        }

        public async Task<SalaResponseDto> FindByCod(string cod) {
            var entity = await _context.Sala.FirstOrDefaultAsync(s => s.Cod.ToLower() == cod.Trim().ToLower());

            if (entity == null) throw new NotFoundException($"No se encontró la Sala con cod {cod}");

            return _mapper.Map<SalaResponseDto>(entity);
        }

        public async Task<SalaResponseDto> FindById(long id) {
            var entity = await _context.Sala.FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null) throw new NotFoundException($"No se encontró la Sala con cod {id}");

            return _mapper.Map<SalaResponseDto>(entity);
        }
    }
}
