using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Menu : BaseEntity
{
    public string Key { get; set; } = string.Empty; // Unique identifier for menu item
    public string? Icon { get; set; } // CSS class or icon name
    public string? Url { get; set; } // Route URL
    public int? ParentId { get; set; } // For hierarchical menu structure
    public int Order { get; set; } = 0; // Display order
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    public ICollection<MenuTranslation> Translations { get; set; } = new List<MenuTranslation>();
}