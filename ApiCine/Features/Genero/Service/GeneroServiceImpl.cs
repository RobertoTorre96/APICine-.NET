using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Genero.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Genero.Service {
    public class GeneroServiceImpl : IGeneroService {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GeneroServiceImpl(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GeneroResponseDto> Create(GeneroRequestDto generoRequestDto) {
            var existe= await _context.Genero.AnyAsync(g=>string.Equals(g.Nombre,generoRequestDto.Nombre));
            if (existe) {
                throw new AlreadyExistsException($"El genero {generoRequestDto.Nombre} ya existe.");
            }

            var generoNuevo = _mapper.Map<GeneroEntity>(generoRequestDto);

            _context.Genero.Add(generoNuevo);
            await _context.SaveChangesAsync();

            return _mapper.Map<GeneroResponseDto>(generoNuevo);
        }

        public async Task<bool> Delete(long id) {
            var genero = await _context.Genero.FindAsync(id);
            if (genero == null) {
                 throw new NotFoundException($"El genero {id} no existe.");
            }
            _context.Genero.Remove(genero);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GeneroResponseDto>> FindAll() {
    
            var generos= await _context.Genero.OrderBy(g=>g.Nombre).ToListAsync();

            return _mapper.Map<IEnumerable<GeneroResponseDto>>(generos);

        }
    }
}
