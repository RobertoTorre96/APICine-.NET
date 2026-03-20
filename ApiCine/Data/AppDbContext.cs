using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApiCine.Data {
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            // Aquí puedes configurar tus entidades y relaciones si es necesario
            // Busca todas las clases que implementan IEntityTypeConfiguration en este proyecto
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
