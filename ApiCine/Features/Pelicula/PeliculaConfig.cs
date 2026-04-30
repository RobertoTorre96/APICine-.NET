using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Pelicula {
    public class PeliculaConfig : IEntityTypeConfiguration<PeliculaEntity> {
        public void Configure(EntityTypeBuilder<PeliculaEntity> builder) {
            builder.ToTable("Pelicula");

            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Codigo)
                .IsUnique();

            builder.Property(p => p.Codigo)
                .IsRequired()
                
                .HasMaxLength(10);

            builder.Property(p => p.Titulo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Duracion)
                .IsRequired();

            builder.Property(p => p.Sinopsis)
                .IsRequired()
                .HasMaxLength(200);
            
        }   
    }
}
