using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class FaturaKalemConfiguration : IEntityTypeConfiguration<FaturaKalem>
{
    public void Configure(EntityTypeBuilder<FaturaKalem> builder)
    {
        builder.ToTable("FaturaKalemleri");

        builder.HasKey(fk => fk.Id);

        builder.Property(fk => fk.Miktar)
            .HasPrecision(18, 2);

        builder.Property(fk => fk.BirimFiyat)
            .HasPrecision(18, 2);

        builder.Property(fk => fk.KdvOrani)
            .HasPrecision(5, 2);

        builder.Property(fk => fk.AraToplam)
            .HasPrecision(18, 2);

        builder.Property(fk => fk.KdvTutari)
            .HasPrecision(18, 2);

        builder.Property(fk => fk.Toplam)
            .HasPrecision(18, 2);

        // Stok relationship
        builder.HasOne(fk => fk.Stok)
            .WithMany(s => s.FaturaKalemleri)
            .HasForeignKey(fk => fk.StokId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
