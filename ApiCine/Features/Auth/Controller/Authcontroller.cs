using ApiCine.Features.Auth.Dto;
using ApiCine.Features.Auth.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiCine.Features.Auth.Controller {
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto) {
            var token = _authService.Login(dto.Email, dto.Password);
            return Ok(new { token });
        }

       
    }
}
