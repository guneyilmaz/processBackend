using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class MenuTranslation : BaseEntity
{
    public int MenuId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; } = string.Empty; // Menu title in specific language
    public string? Description { get; set; } // Optional description
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Menu Menu { get; set; } = null!;
    public Language Language { get; set; } = null!;
}