using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.TaxNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .HasMaxLength(255);

        builder.Property(e => e.Website)
            .HasMaxLength(255);

        builder.Property(e => e.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.TaxNumber)
            .IsUnique();

        builder.HasIndex(e => e.Name);

        // Relationships
        builder.HasMany(e => e.Workplaces)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Employees)
            .WithOne(e => e.Company)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        // Table name
        builder.ToTable("Companies");
    }
}