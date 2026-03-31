using ApiCine.Features.Funcion.DTOs;
using ApiCine.Features.Funcion.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Funcion {

    [ApiController]
    [Route("api/[controller]")]
    public class FuncionController : ControllerBase {
        private readonly IFuncionService _service;

        public FuncionController(IFuncionService service) {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<FuncionResponseDetalleDto>> Create([FromBody] FuncionResquestDto request) {
            // El Service se encarga de validar solapamientos y existencia de Pelicula/Sala
            var result = await _service.Crear(request);

            // Retornamos 201 Created y la URL para consultar esta función específica
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionResponseListaDto>>> GetAll() {
            var result = await _service.FindAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionResponseDetalleDto>> GetById(long id) {
            var result = await _service.FindById(id);
            return Ok(result);
        }

        [HttpGet("pelicula/{peliculaId}")]
        public async Task<ActionResult<IEnumerable<FuncionResponseListaDto>>> GetByPelicula(long peliculaId) {
            var result = await _service.FindByPelicula(peliculaId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            // El Service validará que no tenga reservas antes de borrar
            await _service.Delete(id);
            return NoContent(); // 204 No Content es el estándar para borrados exitosos
        }

    }
}
