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

        }
    }
}
