using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class CariHesapRepository : ICariHesapRepository
{
    private readonly ApplicationDbContext _context;

    public CariHesapRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CariHesap>> GetAllAsync()
    {
        return await _context.CariHesaplar
            .Include(c => c.Company)
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.CariAdi)
            .ToListAsync();
    }

    public async Task<CariHesap?> GetByIdAsync(int id)
    {
        return await _context.CariHesaplar
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<CariHesap?> GetByCariKoduAsync(string cariKodu)
    {
        return await _context.CariHesaplar
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.CariKodu == cariKodu && !c.IsDeleted);
    }

    public async Task<CariHesap> CreateAsync(CariHesap cariHesap)
    {
        cariHesap.CreatedAt = DateTime.UtcNow;
        cariHesap.Bakiye = 0; // Başlangıç bakiyesi sıfır
        _context.CariHesaplar.Add(cariHesap);
        await _context.SaveChangesAsync();
        return cariHesap;
    }

    public async Task<CariHesap> UpdateAsync(CariHesap cariHesap)
    {
        cariHesap.UpdatedAt = DateTime.UtcNow;
        _context.CariHesaplar.Update(cariHesap);
        await _context.SaveChangesAsync();
        return cariHesap;
    }

    public async Task DeleteAsync(int id)
    {
        var cariHesap = await GetByIdAsync(id);
        if (cariHesap != null)
        {
            cariHesap.IsDeleted = true;
            cariHesap.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CariKoduExistsAsync(string cariKodu, int? excludeId = null)
    {
        var query = _context.CariHesaplar.Where(c => c.CariKodu == cariKodu && !c.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task UpdateBakiyeAsync(int cariHesapId, decimal tutar)
    {
        var cariHesap = await GetByIdAsync(cariHesapId);
        if (cariHesap != null)
        {
            cariHesap.Bakiye += tutar;
            cariHesap.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
