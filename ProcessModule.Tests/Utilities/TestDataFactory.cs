using ProcessModule.Domain.Entities;
using ProcessModule.Application.Features.Menus.Queries;

namespace ProcessModule.Tests.Utilities;

/// <summary>
/// Factory class for creating test data objects
/// </summary>
public static class TestDataFactory
{
    public static Language CreateLanguage(int id = 1, string code = "tr", string name = "Türkçe")
    {
        return new Language
        {
            Id = id,
            Code = code,
            Name = name,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Menu CreateMenu(int id = 1, string key = "dashboard", string? icon = "fa fa-home", string? url = "/dashboard", int? parentId = null, int order = 1)
    {
        return new Menu
        {
            Id = id,
            Key = key,
            Icon = icon,
            Url = url,
            ParentId = parentId,
            Order = order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Translations = new List<MenuTranslation>(),
            Children = new List<Menu>()
        };
    }

    public static MenuTranslation CreateMenuTranslation(int id = 1, int menuId = 1, int languageId = 1, string title = "Test Menu", string? description = "Test Description")
    {
        return new MenuTranslation
        {
            Id = id,
            MenuId = menuId,
            LanguageId = languageId,
            Title = title,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            Menu = CreateMenu(menuId),
            Language = CreateLanguage(languageId)
        };
    }

    public static MenuDto CreateMenuDto(int id = 1, string key = "dashboard", string title = "Test Menu", string? description = "Test Description", string? icon = "fa fa-home", string? url = "/dashboard", int? parentId = null, int order = 1)
    {
        return new MenuDto
        {
            Id = id,
            Key = key,
            Title = title,
            Description = description,
            Icon = icon,
            Url = url,
            ParentId = parentId,
            Order = order,
            IsActive = true,
            Children = new List<MenuDto>()
        };
    }

    public static List<Menu> CreateMenusWithTranslations()
    {
        var language = CreateLanguage(1, "tr", "Türkçe");
        
        var menu1 = CreateMenu(1, "dashboard", "fa fa-home", "/dashboard", null, 1);
        var menu2 = CreateMenu(2, "users", "fa fa-users", "/users", null, 2);
        
        menu1.Translations.Add(new MenuTranslation
        {
            Id = 1,
            MenuId = 1,
            LanguageId = 1,
            Title = "Anasayfa",
            Description = "Ana sayfa",
            Language = language,
            Menu = menu1
        });
        
        menu2.Translations.Add(new MenuTranslation
        {
            Id = 2,
            MenuId = 2,
            LanguageId = 1,
            Title = "Kullanıcılar",
            Description = "Kullanıcı yönetimi",
            Language = language,
            Menu = menu2
        });

        return new List<Menu> { menu1, menu2 };
    }
}