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
using System.IO;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE JWT ---
var secretKey = builder.Configuration["JWT_SECRET_KEY"] ?? "TuClaveSuperSecreta123456789";
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

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --- BASE DE DATOS (Ruta Absoluta para evitar errores en Docker) ---
string dbPath = Path.Combine(AppContext.BaseDirectory, "Cine.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] {} }
    });
});

// --- config swagger ---
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// --- INICIALIZACIÓN DE DATOS ---
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    try {
        Console.WriteLine(">>> Iniciando proceso de base de datos...");

        // 1. Recreamos la base de datos
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        Console.WriteLine(">>> Base de datos recreada exitosamente.");

        // 2. Buscamos el archivo SQL en múltiples lugares posibles
        string baseDir = AppContext.BaseDirectory;
        string[] rutasParaProbar = {
            Path.Combine(baseDir, "Data", "Scripts", "SeedData.sql"),
            Path.Combine(baseDir, "SeedData.sql"),
            "/app/Data/Scripts/SeedData.sql",
            "/app/publish/Data/Scripts/SeedData.sql"
        };

        string? rutaEncontrada = rutasParaProbar.FirstOrDefault(ruta => File.Exists(ruta));

        if (rutaEncontrada != null) {
            Console.WriteLine($">>> Archivo encontrado en: {rutaEncontrada}");
            var sql = File.ReadAllText(rutaEncontrada);

            // Ejecutamos el SQL
            context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine(">>> SEED COMPLETADO EXITOSAMENTE.");
        }
        else {
            Console.WriteLine(">>> ERROR: No se encontró SeedData.sql.");
            // Listar archivos para saber dónde quedó realmente
            Console.WriteLine(">>> Contenido del directorio actual:");
            foreach (var f in Directory.GetFiles(baseDir, "*.*", SearchOption.AllDirectories)
                                       .Where(s => s.EndsWith(".sql"))) {
                Console.WriteLine($"--- Encontrado: {f}");
            }
        }
    }
    catch (Exception ex) {
        Console.WriteLine($">>> ERROR CRÍTICO EN SEED: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCine v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();