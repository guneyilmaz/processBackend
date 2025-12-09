using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class MenuRepository : Repository<Menu>, IMenuRepository
{
    private readonly ApplicationDbContext _context;

    public MenuRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Menu>> GetMenusWithTranslationsAsync(string languageCode)
    {
        return await _context.Menus
            .Include(m => m.Translations)
                .ThenInclude(t => t.Language)
            .Include(m => m.Children)
            .Where(m => m.IsActive && !m.IsDeleted)
            .Where(m => m.Translations.Any(t => t.Language.Code == languageCode))
            .OrderBy(m => m.Order)
            .ToListAsync();
    }

    public async Task<List<Menu>> GetActiveMenusAsync()
    {
        return await _context.Menus
            .Include(m => m.Translations)
            .Include(m => m.Children)
            .Where(m => m.IsActive && !m.IsDeleted)
            .OrderBy(m => m.Order)
            .ToListAsync();
    }
}