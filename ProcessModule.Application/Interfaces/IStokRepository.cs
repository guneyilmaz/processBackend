using ProcessModule.Application.DTOs;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface IStokRepository
{
    Task<IEnumerable<Stok>> GetAllAsync();
    Task<Stok?> GetByIdAsync(int id);
    Task<Stok?> GetByStokKoduAsync(string stokKodu);
    Task<Stok> CreateAsync(Stok stok);
    Task<Stok> UpdateAsync(Stok stok);
    Task DeleteAsync(int id);
    Task<bool> StokKoduExistsAsync(string stokKodu, int? excludeId = null);
    Task<IEnumerable<Stok>> GetLowStockItemsAsync();
}
