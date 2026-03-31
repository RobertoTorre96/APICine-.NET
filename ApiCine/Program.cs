using ApiCine.Data;
using ApiCine.Exceptions;
using ApiCine.Features.Funcion.Service;
using ApiCine.Features.Genero.Service;
using ApiCine.Features.Pelicula.Service;
using ApiCine.Features.Reserva.Service;
using ApiCine.Features.Sala.Service;
using ApiCine.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// inyecciones
builder.Services.AddScoped<IGeneroService, GeneroServiceImpl>();
builder.Services.AddScoped<IPeliculaService, PeliculaServiceImpl>();
builder.Services.AddScoped<ISalaService, SalaServiceImpl>();
builder.Services.AddScoped<IFuncionService, FuncionServiceImpl>();
builder.Services.AddScoped<IReservaService, ReservaServiceImpl>();
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
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
