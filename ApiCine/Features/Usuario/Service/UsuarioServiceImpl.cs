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

        public UsuarioServiceImpl(AppDbContext context, IMapper mapper, IConfiguration config) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioResponseDto> Create(UsuarioRequestDto request) {
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

        public async Task<UsuarioResponseDto> FindById(long id) {
            var usuario = await _context.Usuario.FindAsync(id)
                ?? throw new NotFoundException($"No se encontró el usuario con ID: {id}");

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> FindAll() {
            var usuarios = await _context.Usuario.ToListAsync();
            return _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
        }



    }
}
