using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class MenuTranslationConfiguration : IEntityTypeConfiguration<MenuTranslation>
{
    public void Configure(EntityTypeBuilder<MenuTranslation> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.MenuId)
            .IsRequired();

        builder.Property(e => e.LanguageId)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Composite unique index for MenuId + LanguageId
        builder.HasIndex(e => new { e.MenuId, e.LanguageId })
            .IsUnique();

        // Relationships
        builder.HasOne(e => e.Menu)
            .WithMany(e => e.Translations)
            .HasForeignKey(e => e.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Language)
            .WithMany()
            .HasForeignKey(e => e.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Table name
        builder.ToTable("MenuTranslations");
    }
}