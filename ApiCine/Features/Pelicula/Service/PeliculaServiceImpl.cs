using ApiCine.common.DTOs;
using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Pelicula.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Pelicula.Service {


    public class PeliculaServiceImpl : IPeliculaService {


        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PeliculaServiceImpl(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PeliculaResponseDto> Create(PeliculaRequestDto request) {
            string tituloRequest=request.Titulo.Trim().ToLower();
            var existe= _context.Pelicula.AnyAsync(p=>p.Titulo== tituloRequest);
            if (await existe) {
                throw new AlreadyExistsException($"La pelicula {request.Titulo} ya existe.");
            }
            var entity = _mapper.Map<PeliculaEntity>(request);
            _context.Pelicula.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<PeliculaResponseDto>(entity);
        }

        public async Task<PageResponseDto<PeliculaResponseDto>> FindAll(int numeroPagina, int tamPagina) {

            if (numeroPagina <1) numeroPagina = 1;
            if (tamPagina < 1) tamPagina = 10;

            var totalItems = await _context.Pelicula.CountAsync();
            
            var peliculas = await _context.Pelicula
                .OrderBy(p => p.Id)
                .Skip((numeroPagina - 1) * tamPagina)
                .Take(tamPagina)    
                .ToListAsync();


            var response = _mapper.Map<ICollection<PeliculaResponseDto>>(peliculas);

            return new PageResponseDto<PeliculaResponseDto> {
                Items = response,
                numeroPagina = numeroPagina,
                TamPagina = tamPagina,
                TotalItems = totalItems
            };



        }

        public async Task<PeliculaResponseDto> FindById(long id) {
           
            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula == null) {
                 throw new NotFoundException($"Pelicula con id {id} no encontrada");
            }
            return _mapper.Map<PeliculaResponseDto>(pelicula);
        }
    }
}
