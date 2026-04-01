using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Funcion.Service;
using ApiCine.Features.Genero.Service;
using ApiCine.Features.Pelicula.Service;
using ApiCine.Features.Reserva.Service;
using ApiCine.Features.Sala.Service;
using ApiCine.Features.Usuario.Service;
using ApiCine.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Jwt
var secretKey = builder.Configuration["JWT_SECRET_KEY"]
                ?? throw new Exception("Falta la variable JWT_SECRET_KEY");
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(config => {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true, // Validar que la firma sea nuestra
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Verificar que el token no haya expirado
            ClockSkew = TimeSpan.Zero // Para que expire en el segundo exacto
        };
    });


// inyecciones
builder.Services.AddScoped<IGeneroService, GeneroServiceImpl>();
builder.Services.AddScoped<IPeliculaService, PeliculaServiceImpl>();
builder.Services.AddScoped<ISalaService, SalaServiceImpl>();
builder.Services.AddScoped<IFuncionService, FuncionServiceImpl>();
builder.Services.AddScoped<IReservaService, ReservaServiceImpl>();
builder.Services.AddScoped<IUsuarioService, UsuarioServiceImpl>();
//validaciones de DTO
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = context => {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var errorResponse = new {
            StatusCode=400,
            Message = "Validation failed",
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

//globlandHandler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configurar el DbContext para que use SQLite
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    // 1. Definimos CÓMO se llama el esquema de seguridad
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegá tu token JWT aquí (No hace falta escribir 'Bearer', el sistema lo suma solo)."
    });

    // 2. Aplicamos ese esquema de forma GLOBAL a todos los endpoints en Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
