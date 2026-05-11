using ApiCine.Features.Usuario.DTOs;
using ApiCine.Features.Usuario.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Usuario {
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase {

        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService) {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <remarks>
        /// **Instrucciones para el campo Role:**
        /// - Use **1** para asignar el rol de **Cliente**.
        /// - Use **2** para asignar el rol de **Admin**.
        /// </remarks>
        public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioRequestDto request) {
            var resultado = await _usuarioService.Create(request);
            // Retorna un 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(FindById), new { id = resultado.Id }, resultado);
        }




        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> FindById(long id) {
            return Ok(await _usuarioService.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> FindAll() {
            return Ok(await _usuarioService.FindAll());
        }

    }
}
