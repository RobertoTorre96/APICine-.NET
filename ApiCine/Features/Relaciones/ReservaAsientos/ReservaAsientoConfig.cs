using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Relaciones.ReservaAsientos {
    public class ReservaAsientoConfig : IEntityTypeConfiguration<ReservaAsientoEntity> {
        public void Configure(EntityTypeBuilder<ReservaAsientoEntity> builder) {
            builder.ToTable("Reserva_asiento");
            builder.HasKey(x => x.Id);
            builder.HasIndex(ra=>new { ra.FuncionId,ra.AsientoId})
                .IsUnique()
                .HasDatabaseName("IX_ReservaAsiento_FuncionId_AsientoId");

            builder.HasOne(ra => ra.Reserva)
                .WithMany(r => r.reservaAsientos)
                .HasForeignKey(ra => ra.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ra => ra.Asiento)
                .WithMany(a => a.reservaAsientos)
                .HasForeignKey(ra => ra.AsientoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
