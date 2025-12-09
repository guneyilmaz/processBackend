using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private new readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetCompaniesWithWorkplacesAsync()
    {
        return await _context.Companies
            .Include(c => c.Workplaces.Where(w => w.IsActive && !w.IsDeleted))
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyWithWorkplacesAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.Workplaces.Where(w => w.IsActive && !w.IsDeleted))
            .Include(c => c.Employees.Where(e => e.IsActive && !e.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<bool> IsCompanyNameExistsAsync(string name, int? excludeId = null)
    {
        var query = _context.Companies.Where(c => c.Name == name && !c.IsDeleted);
        
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
            
        return await query.AnyAsync();
    }

    public async Task<bool> IsTaxNumberExistsAsync(string taxNumber, int? excludeId = null)
    {
        var query = _context.Companies.Where(c => c.TaxNumber == taxNumber && !c.IsDeleted);
        
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
            
        return await query.AnyAsync();
    }

    public async Task<List<Company>> GetActiveCompaniesAsync()
    {
        return await _context.Companies
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}