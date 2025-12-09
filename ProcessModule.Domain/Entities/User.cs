using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    // Company and Workplace relationships
    public int? CompanyId { get; set; }
    public int? WorkplaceId { get; set; }

    // Navigation properties
    public Company? Company { get; set; }
    public Workplace? Workplace { get; set; }
    public ICollection<Workplace> ManagedWorkplaces { get; set; } = new List<Workplace>(); // Yönettiği işyerleri
}