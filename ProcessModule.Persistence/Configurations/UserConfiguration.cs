using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Password)
            .IsRequired();

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        // Indexes
        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasIndex(e => e.CompanyId);
        builder.HasIndex(e => e.WorkplaceId);

        // Relationships
        builder.HasOne(e => e.Company)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Workplace)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.WorkplaceId)
            .OnDelete(DeleteBehavior.SetNull);

        // Table name (optional - EF Core will use "Users" by default)
        builder.ToTable("Users");
    }
}