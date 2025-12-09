using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class WorkplaceRepository : Repository<Workplace>, IWorkplaceRepository
{
    private new readonly ApplicationDbContext _context;

    public WorkplaceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Workplace>> GetWorkplacesByCompanyAsync(int companyId)
    {
        return await _context.Workplaces
            .Include(w => w.Company)
            .Include(w => w.Manager)
            .Where(w => w.CompanyId == companyId && !w.IsDeleted)
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    public async Task<Workplace?> GetWorkplaceWithDetailsAsync(int id)
    {
        return await _context.Workplaces
            .Include(w => w.Company)
            .Include(w => w.Manager)
            .Include(w => w.Employees.Where(e => e.IsActive && !e.IsDeleted))
            .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
    }

    public async Task<List<Workplace>> GetWorkplacesWithCompanyAsync()
    {
        return await _context.Workplaces
            .Include(w => w.Company)
            .Include(w => w.Manager)
            .Where(w => !w.IsDeleted)
            .OrderBy(w => w.Company.Name)
            .ThenBy(w => w.Name)
            .ToListAsync();
    }

    public async Task<bool> IsWorkplaceNameExistsInCompanyAsync(string name, int companyId, int? excludeId = null)
    {
        var query = _context.Workplaces
            .Where(w => w.Name == name && w.CompanyId == companyId && !w.IsDeleted);
        
        if (excludeId.HasValue)
            query = query.Where(w => w.Id != excludeId.Value);
            
        return await query.AnyAsync();
    }

    public async Task<List<Workplace>> GetActiveWorkplacesAsync()
    {
        return await _context.Workplaces
            .Include(w => w.Company)
            .Where(w => w.IsActive && !w.IsDeleted)
            .OrderBy(w => w.Company.Name)
            .ThenBy(w => w.Name)
            .ToListAsync();
    }
}