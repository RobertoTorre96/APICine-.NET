using ApiCine.common.DTOs;
using ApiCine.Features.Pelicula.DTOs;
using ApiCine.Features.Pelicula.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Pelicula {


    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculaController : ControllerBase {

        private readonly IPeliculaService _peliculaService;

        public PeliculaController(IPeliculaService peliculaService) {
            _peliculaService = peliculaService;
        }

        [HttpPost]
        public async Task<ActionResult<PeliculaResponseDto>> Create([FromBody] PeliculaRequestDto request) {
            var peliculaCreada = await _peliculaService.Create(request);
            return Ok(peliculaCreada);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PageResponseDto<PeliculaResponseDto>>> GetAll([FromQuery] int numeroPagina = 1,
                                                                                     [FromQuery] int tamPagina = 10) {
            var peliculas = await _peliculaService.FindAll(numeroPagina, tamPagina);
            return Ok(peliculas);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaResponseDto>> GetById([FromRoute] long id) {
            var pelicula = await _peliculaService.FindById(id);
            return Ok(pelicula);


        }
    }
}
