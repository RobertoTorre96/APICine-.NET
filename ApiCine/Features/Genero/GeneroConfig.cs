using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Genero {
    public class GeneroConfig : IEntityTypeConfiguration<GeneroEntity> {
        public void Configure(EntityTypeBuilder<GeneroEntity> builder) {
            builder.ToTable("Genero");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Nombre)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(g => g.Nombre)
                .IsUnique();


            builder.HasData(
                new { Id = 1L, Nombre = "Acción" },
                new { Id = 2L, Nombre = "Terror" },
                new { Id = 3L, Nombre = "Ciencia Ficción" }
            );
        }
    }
}
