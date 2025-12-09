using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Workplace : BaseEntity
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public int? ManagerId { get; set; } // İşyeri müdürü
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Company Company { get; set; } = null!;
    public User? Manager { get; set; } // İşyeri müdürü
    public ICollection<User> Employees { get; set; } = new List<User>(); // İşyeri çalışanları
}