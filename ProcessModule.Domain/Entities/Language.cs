using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // Örn: 'tr', 'en', 'de'
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
