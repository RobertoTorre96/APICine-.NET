using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Usuario.DTOs;
using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCine.Features.Usuario.Service {
    public class UsuarioServiceImpl : IUsuarioService {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; // <--- Agregamos esto para leer el JWT_SECRET_KEY

        public UsuarioServiceImpl(AppDbContext context, IMapper mapper, IConfiguration config) {
            _context = context;
            _mapper = mapper;
            _config = config;
        }



        public async Task<string> Login( UsuarioLoginRequestDto request) {
            // 1. Buscamos el usuario por Username
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == request.Email)
                ?? throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            // 2. Verificamos la contraseña usando el Hash de la DB
            bool isPasswordOk = BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password);

            if (!isPasswordOk) {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            }

            // 3. Si todo está bien, generamos el Token
            return GenerarJwtToken(usuario);
        }

        // --- LÓGICA DE GENERACIÓN DEL TOKEN ---
        private string GenerarJwtToken(UsuarioEntity usuario) {
            var secretKey = _config["JWT_SECRET_KEY"]
                            ?? throw new Exception("JWT_SECRET_KEY no encontrada en la configuración");

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(keyBytes);

            // Información que "viaja" dentro del token (Claims)
            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Username),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Role, usuario.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3), // El token dura 3 horas
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }



        public async Task<UsuarioResponseDto> Registrar(UsuarioRequestDto request) {
            // Validar si el email ya existe
            if (await _context.Usuario.AnyAsync(u => u.Email == request.Email)) {
                throw new BadRequestException("El email ya se encuentra registrado.");
            }

            // Validar si el username ya existe
            if (await _context.Usuario.AnyAsync(u => u.Username == request.Username)) {
                throw new BadRequestException("El nombre de usuario ya está en uso.");
            }

            var usuario = _mapper.Map<UsuarioEntity>(request);

            // Hasheo de seguridad para la password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<UsuarioResponseDto> GetById(long id) {
            var usuario = await _context.Usuario.FindAsync(id)
                ?? throw new NotFoundException($"No se encontró el usuario con ID: {id}");

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAll() {
            var usuarios = await _context.Usuario.ToListAsync();
            return _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
        }



    }
}
