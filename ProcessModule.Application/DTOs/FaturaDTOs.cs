using System.ComponentModel.DataAnnotations;

namespace ProcessModule.Application.DTOs;

// Fatura DTOs
public class FaturaDto
{
    public int Id { get; set; }
    public string FaturaNo { get; set; } = string.Empty;
    public DateTime FaturaTarihi { get; set; }
    public int CariHesapId { get; set; }
    public string? CariAdi { get; set; }
    public decimal ToplamTutar { get; set; }
    public decimal KdvTutari { get; set; }
    public decimal GenelToplam { get; set; }
    public decimal OncekiBakiye { get; set; }
    public decimal SonrakiBakiye { get; set; }
    public string? Aciklama { get; set; }
    public string? PdfPath { get; set; }
    public List<FaturaKalemDto> FaturaKalemleri { get; set; } = new();
}

public class FaturaKalemDto
{
    public int Id { get; set; }
    public int FaturaId { get; set; }
    public int StokId { get; set; }
    public string? StokAdi { get; set; }
    public string? Birimi { get; set; }
    public decimal Miktar { get; set; }
    public decimal BirimFiyat { get; set; }
    public decimal KdvOrani { get; set; }
    public decimal AraToplam { get; set; }
    public decimal KdvTutari { get; set; }
    public decimal Toplam { get; set; }
}

public class CreateFaturaDto
{
    [Required(ErrorMessage = "Fatura numarası zorunludur")]
    [StringLength(50)]
    public string FaturaNo { get; set; } = string.Empty;

    public DateTime FaturaTarihi { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Cari hesap seçilmelidir")]
    public int CariHesapId { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }

    [Required(ErrorMessage = "En az bir kalem eklenmelidir")]
    [MinLength(1, ErrorMessage = "En az bir kalem eklenmelidir")]
    public List<CreateFaturaKalemDto> FaturaKalemleri { get; set; } = new();
}

public class CreateFaturaKalemDto
{
    [Required(ErrorMessage = "Stok seçilmelidir")]
    public int StokId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır")]
    public decimal Miktar { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır")]
    public decimal BirimFiyat { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "KDV oranı 0-100 arasında olmalıdır")]
    public decimal KdvOrani { get; set; } = 20;
}

public class UpdateFaturaDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Fatura numarası zorunludur")]
    [StringLength(50)]
    public string FaturaNo { get; set; } = string.Empty;

    public DateTime FaturaTarihi { get; set; }

    [Required(ErrorMessage = "Cari hesap seçilmelidir")]
    public int CariHesapId { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }
}
