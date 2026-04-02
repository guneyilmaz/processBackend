using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Stok : BaseEntity
{
    public string StokKodu { get; set; } = string.Empty;
    public string StokAdi { get; set; } = string.Empty;
    public string Birimi { get; set; } = string.Empty; // Adet, Kg, Lt, Mt vb.
    public decimal BirimFiyati { get; set; }
    public decimal MevcutAdet { get; set; }
    public decimal? MinimumStokAdedi { get; set; }
    public string? Aciklama { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<FaturaKalem> FaturaKalemleri { get; set; } = new List<FaturaKalem>();
}
