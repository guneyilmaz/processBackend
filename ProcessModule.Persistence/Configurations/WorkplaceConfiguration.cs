using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class WorkplaceConfiguration : IEntityTypeConfiguration<Workplace>
{
    public void Configure(EntityTypeBuilder<Workplace> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.CompanyId)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => new { e.CompanyId, e.Name })
            .IsUnique();

        builder.HasIndex(e => e.ManagerId);

        // Relationships
        builder.HasOne(e => e.Company)
            .WithMany(e => e.Workplaces)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Manager)
            .WithMany(e => e.ManagedWorkplaces)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.Employees)
            .WithOne(e => e.Workplace)
            .HasForeignKey(e => e.WorkplaceId)
            .OnDelete(DeleteBehavior.SetNull);

        // Table name
        builder.ToTable("Workplaces");
    }
}