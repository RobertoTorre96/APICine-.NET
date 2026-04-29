using ApiCine.Data;
using ApiCine.Features.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCine.Features.Auth.Service {
    public class AuthService : IAuthService {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config) {
            _context = context;
            _config = config;
        }

        public string Login(string email, string password) {
            var user = _context.Usuario.FirstOrDefault(x => x.Email == email);

            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Password incorrecto");

            return GenerateToken(user);
        }

        private string GenerateToken(UsuarioEntity user) {
            var key = Encoding.UTF8.GetBytes(_config["JWT_SECRET_KEY"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
