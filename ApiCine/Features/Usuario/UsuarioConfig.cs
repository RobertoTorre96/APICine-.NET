using ApiCine.Features.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCine.Features.Usuario {
    public class UsuarioConfig : IEntityTypeConfiguration<UsuarioEntity> {
        public void Configure(EntityTypeBuilder<UsuarioEntity> builder) {
            builder.ToTable("Usuario");
            builder.HasKey(x => x.Id);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Role).IsRequired().HasConversion<string>();

          
        }


      
    }
}
