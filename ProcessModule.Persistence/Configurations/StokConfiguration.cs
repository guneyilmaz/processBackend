using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class StokConfiguration : IEntityTypeConfiguration<Stok>
{
    public void Configure(EntityTypeBuilder<Stok> builder)
    {
        builder.ToTable("Stoklar");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StokKodu)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.StokAdi)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Birimi)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.BirimFiyati)
            .HasPrecision(18, 2);

        builder.Property(s => s.MevcutAdet)
            .HasPrecision(18, 2);

        builder.Property(s => s.MinimumStokAdedi)
            .HasPrecision(18, 2);

        builder.Property(s => s.Aciklama)
            .HasMaxLength(500);

        builder.HasIndex(s => s.StokKodu).IsUnique();
    }
}
