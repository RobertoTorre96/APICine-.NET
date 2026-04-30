using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Auth.Service;
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
using System.IO; // Necesario para leer el SQL

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE JWT ---
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
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// --- INYECCIÓN DE DEPENDENCIAS ---
builder.Services.AddScoped<IGeneroService, GeneroServiceImpl>();
builder.Services.AddScoped<IPeliculaService, PeliculaServiceImpl>();
builder.Services.AddScoped<ISalaService, SalaServiceImpl>();
builder.Services.AddScoped<IFuncionService, FuncionServiceImpl>();
builder.Services.AddScoped<IReservaService, ReservaServiceImpl>();
builder.Services.AddScoped<IUsuarioService, UsuarioServiceImpl>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- VALIDACIONES DE DTO ---
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = context => {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var errorResponse = new {
            StatusCode = 400,
            Message = "Validation failed",
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

// --- MANEJO GLOBAL DE EXCEPCIONES ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// --- AUTOMAPPER ---
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --- BASE DE DATOS (SQLite) ---
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- CONFIGURACIÓN DE SWAGGER ---
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegá tu token JWT aquí."
    });

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

// --- LÓGICA DE INICIALIZACIÓN (DATABASE & SQL SEEDING) ---
// --- LÓGICA DE INICIALIZACIÓN (Versión Final Definitiva) ---
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    try {
        // 1. FORZAMOS EL RESET: Borra el archivo .db viejo y lo crea de cero con todas las tablas
        Console.WriteLine(">>> Eliminando base de datos antigua...");
        context.Database.EnsureDeleted();

        Console.WriteLine(">>> Creando tablas nuevas desde los modelos C#...");
        context.Database.EnsureCreated();

        // 2. BUSCAMOS EL SCRIPT (Ruta corregida para Docker)
        // Probamos con la ruta relativa que suele funcionar en la raíz de la app publicada
        var sqlPath = Path.Combine(AppContext.BaseDirectory, "Data", "Scripts", "SeedData.sql");

        if (File.Exists(sqlPath)) {
            Console.WriteLine(">>> Ejecutando SeedData.sql...");
            var sql = File.ReadAllText(sqlPath);
            context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine(">>> SEED COMPLETADO EXITOSAMENTE.");
        }
        else {
            // Si falla la ruta anterior, probamos la ruta de desarrollo por si acaso
            var altPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scripts", "SeedData.sql");
            if (File.Exists(altPath)) {
                var sql = File.ReadAllText(altPath);
                context.Database.ExecuteSqlRaw(sql);
                Console.WriteLine(">>> SEED COMPLETADO (Ruta alternativa).");
            }
            else {
                Console.WriteLine(">>> ERROR: El archivo SQL sigue sin aparecer en ninguna ruta.");
            }
        }
    }
    catch (Exception ex) {
        Console.WriteLine($">>> ERROR CRÍTICO: {ex.Message}");
    }
}

// --- MIDDLEWARE PIPELINE ---

// Swagger habilitado para producción en Render
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCine v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();