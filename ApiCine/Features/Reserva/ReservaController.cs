using ApiCine.Features.Reserva.DTOs;
using ApiCine.Features.Reserva.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Reserva {
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase {

        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService) {
            _reservaService = reservaService;
        }

        // POST: api/reserva
        [HttpPost]
        public async Task<ActionResult<ReservaResponseDto>> Post([FromBody] ReservaRequestDto request) {
            // Gracias a tus Exceptions personalizadas, si algo falla, 
            // el Middleware devolverá el mensaje correcto (400 o 404).
            var resultado = await _reservaService.RealizarReserva(request);

            // Retornamos 201 Created con la ruta para consultar la reserva
            return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
        }

        // GET: api/reserva/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaResponseDto>> GetById(long id) {
            var reserva = await _reservaService.GetById(id);
            return Ok(reserva);
        }

        // GET: api/reserva/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetByUsuario(long usuarioId) {
            var reservas = await _reservaService.GetByUsuario(usuarioId);
            return Ok(reservas);
        }

        // PATCH: api/reserva/{id}/cancelar
        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult> Cancelar(long id) {
            await _reservaService.CancelarReserva(id);
            return NoContent(); // 204: Operación exitosa sin contenido de retorno
        }

    }
}
