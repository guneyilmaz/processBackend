using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class StokRepository : IStokRepository
{
    private readonly ApplicationDbContext _context;

    public StokRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stok>> GetAllAsync()
    {
        return await _context.Stoklar
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.StokAdi)
            .ToListAsync();
    }

    public async Task<Stok?> GetByIdAsync(int id)
    {
        return await _context.Stoklar
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<Stok?> GetByStokKoduAsync(string stokKodu)
    {
        return await _context.Stoklar
            .FirstOrDefaultAsync(s => s.StokKodu == stokKodu && !s.IsDeleted);
    }

    public async Task<Stok> CreateAsync(Stok stok)
    {
        stok.CreatedAt = DateTime.UtcNow;
        _context.Stoklar.Add(stok);
        await _context.SaveChangesAsync();
        return stok;
    }

    public async Task<Stok> UpdateAsync(Stok stok)
    {
        stok.UpdatedAt = DateTime.UtcNow;
        _context.Stoklar.Update(stok);
        await _context.SaveChangesAsync();
        return stok;
    }

    public async Task DeleteAsync(int id)
    {
        var stok = await GetByIdAsync(id);
        if (stok != null)
        {
            stok.IsDeleted = true;
            stok.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> StokKoduExistsAsync(string stokKodu, int? excludeId = null)
    {
        var query = _context.Stoklar.Where(s => s.StokKodu == stokKodu && !s.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(s => s.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Stok>> GetLowStockItemsAsync()
    {
        return await _context.Stoklar
            .Where(s => !s.IsDeleted && 
                        s.IsActive && 
                        s.MinimumStokAdedi.HasValue && 
                        s.MevcutAdet <= s.MinimumStokAdedi.Value)
            .OrderBy(s => s.StokAdi)
            .ToListAsync();
    }
}
