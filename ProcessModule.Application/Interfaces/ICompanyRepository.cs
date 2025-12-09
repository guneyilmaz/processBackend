using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface ICompanyRepository : IRepository<Company>
{
    Task<List<Company>> GetCompaniesWithWorkplacesAsync();
    Task<Company?> GetCompanyWithWorkplacesAsync(int id);
    Task<bool> IsCompanyNameExistsAsync(string name, int? excludeId = null);
    Task<bool> IsTaxNumberExistsAsync(string taxNumber, int? excludeId = null);
    Task<List<Company>> GetActiveCompaniesAsync();
}