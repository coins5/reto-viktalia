using Clientes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("CLIENTES");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Ruc)
            .HasColumnName("RUC")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(c => c.RazonSocial)
            .HasColumnName("RAZON_SOCIAL")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.TelefonoContacto)
            .HasColumnName("TELEFONO")
            .HasMaxLength(20);

        builder.Property(c => c.CorreoContacto)
            .HasColumnName("CORREO")
            .HasMaxLength(150);

        builder.Property(c => c.Direccion)
            .HasColumnName("DIRECCION")
            .HasMaxLength(200);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("CREATED_AT")
            .HasColumnType("TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("UPDATED_AT")
            .HasColumnType("TIMESTAMP");

        builder.Property(c => c.IsDeleted)
            .HasColumnName("IS_DELETED")
            .HasColumnType("BOOLEAN")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasIndex(c => c.Ruc)
            .IsUnique()
            .HasDatabaseName("UX_CLIENTES_RUC");

        builder.HasIndex(c => c.RazonSocial)
            .HasDatabaseName("IX_CLIENTES_RAZON_SOCIAL");

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
