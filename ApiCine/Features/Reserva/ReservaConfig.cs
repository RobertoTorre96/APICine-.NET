using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Reserva {
    public class ReservaConfig : IEntityTypeConfiguration<ReservaEntity> {
        public void Configure(EntityTypeBuilder<ReservaEntity> builder) {
            builder.ToTable("Reserva");
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Cod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Fecha)
                .IsRequired();

          

            builder.HasOne(r => r.Funcion)
                .WithMany(f => f.Reservas)
                .HasForeignKey(r=>r.FuncionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r=>r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.reservaAsientos)
                .WithOne(ra => ra.Reserva)
                .HasForeignKey(ra=>ra.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
