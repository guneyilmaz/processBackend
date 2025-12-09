using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty; // Vergi numarası
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Workplace> Workplaces { get; set; } = new List<Workplace>();
    public ICollection<User> Employees { get; set; } = new List<User>(); // Firma çalışanları
}