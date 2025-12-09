using MediatR;

namespace ProcessModule.Application.Features.Menus.Queries;

public class GetMenusQuery : IRequest<List<MenuDto>>
{
    public string LanguageCode { get; set; } = "tr"; // Default Turkish
}

public class MenuDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public List<MenuDto> Children { get; set; } = new List<MenuDto>();
}