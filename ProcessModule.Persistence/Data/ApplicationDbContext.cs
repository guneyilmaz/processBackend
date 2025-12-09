using Microsoft.EntityFrameworkCore;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Configurations;

namespace ProcessModule.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<MenuTranslation> MenuTranslations { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Workplace> Workplaces { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new LanguageConfiguration());
        modelBuilder.ApplyConfiguration(new MenuConfiguration());
        modelBuilder.ApplyConfiguration(new MenuTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new WorkplaceConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is Domain.Common.BaseEntity entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}