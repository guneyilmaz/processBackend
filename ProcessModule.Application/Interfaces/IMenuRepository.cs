using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface IMenuRepository : IRepository<Menu>
{
    Task<List<Menu>> GetMenusWithTranslationsAsync(string languageCode);
    Task<List<Menu>> GetActiveMenusAsync();
}