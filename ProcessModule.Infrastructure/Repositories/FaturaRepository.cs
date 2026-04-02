using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class FaturaRepository : IFaturaRepository
{
    private readonly ApplicationDbContext _context;

    public FaturaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Fatura>> GetAllAsync()
    {
        return await _context.Faturalar
            .Include(f => f.CariHesap)
            .Include(f => f.FaturaKalemleri)
                .ThenInclude(fk => fk.Stok)
            .Where(f => !f.IsDeleted)
            .OrderByDescending(f => f.FaturaTarihi)
            .ToListAsync();
    }

    public async Task<Fatura?> GetByIdAsync(int id)
    {
        return await _context.Faturalar
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
    }

    public async Task<Fatura?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Faturalar
            .Include(f => f.CariHesap)
            .Include(f => f.FaturaKalemleri)
                .ThenInclude(fk => fk.Stok)
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
    }

    public async Task<Fatura?> GetByFaturaNoAsync(string faturaNo)
    {
        return await _context.Faturalar
            .Include(f => f.CariHesap)
            .Include(f => f.FaturaKalemleri)
                .ThenInclude(fk => fk.Stok)
            .FirstOrDefaultAsync(f => f.FaturaNo == faturaNo && !f.IsDeleted);
    }

    public async Task<Fatura> CreateAsync(Fatura fatura)
    {
        fatura.CreatedAt = DateTime.UtcNow;
        _context.Faturalar.Add(fatura);
        await _context.SaveChangesAsync();
        return fatura;
    }

    public async Task<Fatura> UpdateAsync(Fatura fatura)
    {
        fatura.UpdatedAt = DateTime.UtcNow;
        _context.Faturalar.Update(fatura);
        await _context.SaveChangesAsync();
        return fatura;
    }

    public async Task DeleteAsync(int id)
    {
        var fatura = await GetByIdAsync(id);
        if (fatura != null)
        {
            fatura.IsDeleted = true;
            fatura.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> FaturaNoExistsAsync(string faturaNo, int? excludeId = null)
    {
        var query = _context.Faturalar.Where(f => f.FaturaNo == faturaNo && !f.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(f => f.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Fatura>> GetByCariHesapIdAsync(int cariHesapId)
    {
        return await _context.Faturalar
            .Include(f => f.CariHesap)
            .Include(f => f.FaturaKalemleri)
                .ThenInclude(fk => fk.Stok)
            .Where(f => f.CariHesapId == cariHesapId && !f.IsDeleted)
            .OrderByDescending(f => f.FaturaTarihi)
            .ToListAsync();
    }

    public async Task<string> GenerateNextFaturaNoAsync()
    {
        var today = DateTime.Today;
        var year = today.Year;
        var month = today.Month;
        
        var prefix = $"F{year}{month:D2}";
        
        var lastFatura = await _context.Faturalar
            .Where(f => f.FaturaNo.StartsWith(prefix))
            .OrderByDescending(f => f.FaturaNo)
            .FirstOrDefaultAsync();

        if (lastFatura == null)
        {
            return $"{prefix}0001";
        }

        var lastNumber = int.Parse(lastFatura.FaturaNo.Substring(prefix.Length));
        var nextNumber = lastNumber + 1;
        
        return $"{prefix}{nextNumber:D4}";
    }
}
