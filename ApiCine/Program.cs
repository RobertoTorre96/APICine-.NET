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
using System.IO;

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

var app = builder.Build();

// --- INICIALIZACIÓN DE DATOS ---
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    try {
        Console.WriteLine($">>> Base de Datos en: {dbPath}");
        context.Database.EnsureDeleted(); // Reset para limpiar tablas mal creadas
        context.Database.EnsureCreated();

        var sqlPath = Path.Combine(AppContext.BaseDirectory, "Data", "Scripts", "SeedData.sql");

        if (File.Exists(sqlPath)) {
            var sql = File.ReadAllText(sqlPath);
            context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine(">>> SEED COMPLETADO EXITOSAMENTE.");
        }
        else {
            Console.WriteLine($">>> ERROR: No se encontró el SQL en {sqlPath}");
        }
    }
    catch (Exception ex) {
        Console.WriteLine($">>> ERROR EN DB: {ex.Message}");
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