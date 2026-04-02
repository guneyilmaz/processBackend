using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
{
    public void Configure(EntityTypeBuilder<Fatura> builder)
    {
        builder.ToTable("Faturalar");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FaturaNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.ToplamTutar)
            .HasPrecision(18, 2);

        builder.Property(f => f.KdvTutari)
            .HasPrecision(18, 2);

        builder.Property(f => f.GenelToplam)
            .HasPrecision(18, 2);

        builder.Property(f => f.OncekiBakiye)
            .HasPrecision(18, 2);

        builder.Property(f => f.SonrakiBakiye)
            .HasPrecision(18, 2);

        builder.Property(f => f.Aciklama)
            .HasMaxLength(500);

        builder.Property(f => f.PdfPath)
            .HasMaxLength(500);

        builder.HasIndex(f => f.FaturaNo).IsUnique();

        // CariHesap relationship
        builder.HasOne(f => f.CariHesap)
            .WithMany(c => c.Faturalar)
            .HasForeignKey(f => f.CariHesapId)
            .OnDelete(DeleteBehavior.Restrict);

        // FaturaKalemleri relationship
        builder.HasMany(f => f.FaturaKalemleri)
            .WithOne(fk => fk.Fatura)
            .HasForeignKey(fk => fk.FaturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
