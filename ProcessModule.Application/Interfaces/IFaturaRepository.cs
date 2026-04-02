using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface IFaturaRepository
{
    Task<IEnumerable<Fatura>> GetAllAsync();
    Task<Fatura?> GetByIdAsync(int id);
    Task<Fatura?> GetByIdWithDetailsAsync(int id);
    Task<Fatura?> GetByFaturaNoAsync(string faturaNo);
    Task<Fatura> CreateAsync(Fatura fatura);
    Task<Fatura> UpdateAsync(Fatura fatura);
    Task DeleteAsync(int id);
    Task<bool> FaturaNoExistsAsync(string faturaNo, int? excludeId = null);
    Task<IEnumerable<Fatura>> GetByCariHesapIdAsync(int cariHesapId);
    Task<string> GenerateNextFaturaNoAsync();
}
