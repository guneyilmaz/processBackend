using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class CariHesap : BaseEntity
{
    public string CariKodu { get; set; } = string.Empty;
    public string CariAdi { get; set; } = string.Empty;
    public string? VergiNumarasi { get; set; }
    public string? VergiDairesi { get; set; }
    public string? Adres { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public string? YetkiliKisi { get; set; }
    public decimal Bakiye { get; set; } = 0; // Pozitif: alacak, Negatif: borç
    public bool IsActive { get; set; } = true;

    // Company relationship
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }

    // Navigation properties
    public ICollection<Fatura> Faturalar { get; set; } = new List<Fatura>();
}
