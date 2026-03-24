using ApiCine.Features.Pelicula;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ApiCine.Features.Funcion {
    public class FuncionConfig : IEntityTypeConfiguration<FuncionEntity>{
    
        public void Configure(EntityTypeBuilder<FuncionEntity> builder) {

            builder.ToTable("funcion");
            builder.HasIndex(f => new { f.FechaHora, f.SalaId })
                .IsUnique()
                .HasDatabaseName("IX_Funcion_FechaHora_SalaId");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Precio)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(f => f.FechaHora)
                .IsRequired();

            builder.HasOne(f => f.Pelicula)
                   .WithMany(p=>p.Funciones)
                   .HasForeignKey(f => f.PeliculaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f=>f.Sala)
                .WithMany(s=>s.Funciones)
                .HasForeignKey(f=>f.SalaId)
                .OnDelete(DeleteBehavior.Restrict);

            
            
        }
    }
}
