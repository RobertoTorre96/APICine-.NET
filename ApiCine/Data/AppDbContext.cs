using ApiCine.Features.Genero;
using ApiCine.Features.Pelicula;
using ApiCine.Features.Relaciones.PeliculaGenero;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApiCine.Data {
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public  DbSet<PeliculaEntity> Pelicula { get; set; }
        public DbSet<GeneroEntity> Genero { get; set; }

        public DbSet<PeliculaGeneroEntity> PeliculaGenero { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            // Aquí puedes configurar tus entidades y relaciones si es necesario
            // Busca todas las clases que implementan IEntityTypeConfiguration en este proyecto
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
