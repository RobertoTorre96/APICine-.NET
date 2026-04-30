using ApiCine.Features.Sala.DTOs;
using ApiCine.Features.Sala.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Sala {


    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase {

        private readonly ISalaService _service;
        public SalaController(ISalaService service) {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<SalaResponseDto>> Crear([FromBody] SalaRequestDto request) {
            var response = await _service.Crear(request);
            return CreatedAtAction(nameof(FindById), new { id = response.Id }, response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaResponseDto>>> FindAll() {
            var response = await _service.FindAll();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<SalaResponseDto>> FindById(long id) {
            var response = await _service.FindById(id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id) {
            await _service.Delete(id);
            return NoContent();

        }
    }

}
