using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Icon)
            .HasMaxLength(50);

        builder.Property(e => e.Url)
            .HasMaxLength(500);

        builder.Property(e => e.Order)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.Key)
            .IsUnique();

        builder.HasIndex(e => e.Order);

        // Self-referencing relationship for hierarchical structure
        builder.HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with MenuTranslation
        builder.HasMany(e => e.Translations)
            .WithOne(e => e.Menu)
            .HasForeignKey(e => e.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        // Table name
        builder.ToTable("Menus");
    }
}