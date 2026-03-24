using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Sala {
    public class SalaConfig : IEntityTypeConfiguration<SalaEntity> {
        public void Configure(EntityTypeBuilder<SalaEntity> builder) {
            builder.ToTable("Sala");
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.Cod)
                .IsUnique()
                .HasDatabaseName("IX_Sala_Cod");

            builder.Property(s => s.Cod)
                .IsRequired()
                .HasMaxLength(50);
            
        }
    }
}
