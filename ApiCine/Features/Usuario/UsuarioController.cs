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
        public async Task<ActionResult<UsuarioResponseDto>> Registrar([FromBody] UsuarioRequestDto request) {
            var resultado = await _usuarioService.Registrar(request);
            // Retorna un 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
        }


        [HttpPost("login")] // La ruta final será api/usuario/login
        [AllowAnonymous]    // Cualquiera puede intentar loguearse
        public async Task<ActionResult<object>> Login([FromBody] UsuarioLoginRequestDto request) {
            // El Service devuelve el string del Token
            var token = await _usuarioService.Login(request);

            // Retornamos un objeto anónimo con el token
            return Ok(new {
                Token = token,
                Type = "Bearer"
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(long id) {
            return Ok(await _usuarioService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll() {
            return Ok(await _usuarioService.GetAll());
        }

    }
}
