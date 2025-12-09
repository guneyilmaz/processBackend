using MediatR;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Menus.Queries;

public class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, List<MenuDto>>
{
    private readonly IMenuRepository _menuRepository;

    public GetMenusQueryHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<List<MenuDto>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        var menus = await _menuRepository.GetMenusWithTranslationsAsync(request.LanguageCode);
        
        var menuDtos = menus.Select(m => new MenuDto
        {
            Id = m.Id,
            Key = m.Key,
            Title = m.Translations.FirstOrDefault(t => t.Language.Code == request.LanguageCode)?.Title ?? m.Key,
            Description = m.Translations.FirstOrDefault(t => t.Language.Code == request.LanguageCode)?.Description,
            Icon = m.Icon,
            Url = m.Url,
            ParentId = m.ParentId,
            Order = m.Order,
            IsActive = m.IsActive
        }).ToList();

        // Build hierarchical structure
        var rootMenus = menuDtos.Where(m => m.ParentId == null).OrderBy(m => m.Order).ToList();
        
        foreach (var rootMenu in rootMenus)
        {
            BuildMenuHierarchy(rootMenu, menuDtos);
        }

        return rootMenus;
    }

    private void BuildMenuHierarchy(MenuDto parentMenu, List<MenuDto> allMenus)
    {
        var children = allMenus
            .Where(m => m.ParentId == parentMenu.Id)
            .OrderBy(m => m.Order)
            .ToList();

        parentMenu.Children = children;

        foreach (var child in children)
        {
            BuildMenuHierarchy(child, allMenus);
        }
    }
}