using Microsoft.EntityFrameworkCore;

namespace ApiCine.Features.Relaciones.PeliculaGenero {
    public class PeliculaGeneroConfig : IEntityTypeConfiguration<PeliculaGeneroEntity> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PeliculaGeneroEntity> builder) {

            builder.ToTable("Pelicula_Genero");
            builder.HasKey(pg => new { pg.PeliculaId, pg.GeneroId });

            builder.HasOne(pg=>pg.Pelicula)
                   .WithMany(p=>p.PeliculaGeneros)
                   .HasForeignKey(p=>p.PeliculaId)
                   .onDelete(DeleteBehavior.Cascade);

            builder.HasOne(pg => pg.Genero)
                   .WithMany(g => g.PeliculaGeneros)
                   .HasForeignKey(g => g.GeneroId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
