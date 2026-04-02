using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class CariHesapConfiguration : IEntityTypeConfiguration<CariHesap>
{
    public void Configure(EntityTypeBuilder<CariHesap> builder)
    {
        builder.ToTable("CariHesaplar");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CariKodu)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CariAdi)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.VergiNumarasi)
            .HasMaxLength(20);

        builder.Property(c => c.VergiDairesi)
            .HasMaxLength(100);

        builder.Property(c => c.Adres)
            .HasMaxLength(500);

        builder.Property(c => c.Telefon)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.YetkiliKisi)
            .HasMaxLength(100);

        builder.Property(c => c.Bakiye)
            .HasPrecision(18, 2);

        builder.HasIndex(c => c.CariKodu).IsUnique();

        // Company relationship
        builder.HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
