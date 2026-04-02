using ProcessModule.Application.DTOs;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface ICariHesapRepository
{
    Task<IEnumerable<CariHesap>> GetAllAsync();
    Task<CariHesap?> GetByIdAsync(int id);
    Task<CariHesap?> GetByCariKoduAsync(string cariKodu);
    Task<CariHesap> CreateAsync(CariHesap cariHesap);
    Task<CariHesap> UpdateAsync(CariHesap cariHesap);
    Task DeleteAsync(int id);
    Task<bool> CariKoduExistsAsync(string cariKodu, int? excludeId = null);
    Task UpdateBakiyeAsync(int cariHesapId, decimal tutar);
}
