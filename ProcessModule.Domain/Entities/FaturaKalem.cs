using ProcessModule.Domain.Common;

namespace ProcessModule.Domain.Entities;

public class FaturaKalem : BaseEntity
{
    public int FaturaId { get; set; }
    public Fatura? Fatura { get; set; }
    
    public int StokId { get; set; }
    public Stok? Stok { get; set; }
    
    public decimal Miktar { get; set; }
    public decimal BirimFiyat { get; set; }
    public decimal KdvOrani { get; set; } = 20; // Varsayılan %20 KDV
    
    public decimal AraToplam { get; set; } // Miktar * BirimFiyat
    public decimal KdvTutari { get; set; } // AraToplam * KdvOrani / 100
    public decimal Toplam { get; set; } // AraToplam + KdvTutari
}
