using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface IWorkplaceRepository : IRepository<Workplace>
{
    Task<List<Workplace>> GetWorkplacesByCompanyAsync(int companyId);
    Task<Workplace?> GetWorkplaceWithDetailsAsync(int id);
    Task<List<Workplace>> GetWorkplacesWithCompanyAsync();
    Task<bool> IsWorkplaceNameExistsInCompanyAsync(string name, int companyId, int? excludeId = null);
    Task<List<Workplace>> GetActiveWorkplacesAsync();
}