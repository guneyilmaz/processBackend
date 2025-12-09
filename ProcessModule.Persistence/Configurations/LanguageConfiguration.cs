using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.Code)
            .IsUnique();

        // Table name
        builder.ToTable("Languages");
    }
}