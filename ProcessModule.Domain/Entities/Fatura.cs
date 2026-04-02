using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class Fatura : BaseEntity
{
    public string FaturaNo { get; set; } = string.Empty;
    public DateTime FaturaTarihi { get; set; } = DateTime.UtcNow;
    public int CariHesapId { get; set; }
    public CariHesap? CariHesap { get; set; }
    
    public decimal ToplamTutar { get; set; }
    public decimal KdvTutari { get; set; }
    public decimal GenelToplam { get; set; }
    
    public decimal OncekiBakiye { get; set; } // Fatura kesilmeden önceki cari bakiyesi
    public decimal SonrakiBakiye { get; set; } // Fatura kesildikten sonraki cari bakiyesi
    
    public string? Aciklama { get; set; }
    public string? PdfPath { get; set; } // PDF dosya yolu
    
    // Navigation properties
    public ICollection<FaturaKalem> FaturaKalemleri { get; set; } = new List<FaturaKalem>();
}
