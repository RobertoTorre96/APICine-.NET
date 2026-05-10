using ApiCine.Features.Genero.DTOs;
using ApiCine.Features.Genero.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Genero {

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class GeneroController : ControllerBase {

        private readonly IGeneroService _generoService;
        public GeneroController(IGeneroService generoService) {
            _generoService = generoService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FindAll() {
            var generos = await _generoService.FindAll();
            return Ok(generos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GeneroRequestDto generoRequestDto) {
            var generoCreado = await _generoService.Create(generoRequestDto);
            return CreatedAtAction(nameof(FindAll), new { id = generoCreado.Id }, generoCreado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id) {
            await _generoService.Delete(id);
            return NoContent();

        }
    }
}
