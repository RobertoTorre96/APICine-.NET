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
        Console.WriteLine(">>> Iniciando creación de base de datos...");

        // 1. Borramos y creamos para asegurar limpieza (Solo si estás en pruebas)
        // context.Database.EnsureDeleted(); // Opcional: usalo si querés resetear todo

        context.Database.EnsureCreated();

        // 2. Verificamos qué tablas existen realmente (Log para depuración)
        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
        context.Database.OpenConnection();
        using var reader = command.ExecuteReader();
        Console.WriteLine(">>> Tablas detectadas en SQLite:");
        while (reader.Read()) {
            Console.WriteLine($" - {reader.GetString(0)}");
        }

        // 3. SEEDING MANUAL: Si la tabla Pelicula está vacía, insertamos el SQL
        // IMPORTANTE: Asegurate de usar el nombre exacto que EF Core le puso a la tabla
        // Si en el log anterior no aparece "Pelicula", cambialo por el que aparezca.

        var sqlPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scripts", "SeedData.sql");
        if (File.Exists(sqlPath)) {
            var sql = File.ReadAllText(sqlPath);
            context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine(">>> Script SeedData.sql ejecutado.");
        }
    }
    catch (Exception ex) {
        Console.WriteLine($">>> ERROR CRÍTICO EN DB: {ex.Message}");
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