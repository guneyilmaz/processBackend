using System.ComponentModel.DataAnnotations;

namespace ProcessModule.Application.DTOs;

// Stok DTOs
public class StokDto
{
    public int Id { get; set; }
    public string StokKodu { get; set; } = string.Empty;
    public string StokAdi { get; set; } = string.Empty;
    public string Birimi { get; set; } = string.Empty;
    public decimal BirimFiyati { get; set; }
    public decimal MevcutAdet { get; set; }
    public decimal? MinimumStokAdedi { get; set; }
    public string? Aciklama { get; set; }
    public bool IsActive { get; set; }
}

public class CreateStokDto
{
    [Required(ErrorMessage = "Stok kodu zorunludur")]
    [StringLength(50)]
    public string StokKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stok adı zorunludur")]
    [StringLength(200)]
    public string StokAdi { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birim zorunludur")]
    [StringLength(20)]
    public string Birimi { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyatı 0'dan büyük olmalıdır")]
    public decimal BirimFiyati { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Mevcut adet negatif olamaz")]
    public decimal MevcutAdet { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum stok adedi negatif olamaz")]
    public decimal? MinimumStokAdedi { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }
}

public class UpdateStokDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Stok kodu zorunludur")]
    [StringLength(50)]
    public string StokKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stok adı zorunludur")]
    [StringLength(200)]
    public string StokAdi { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birim zorunludur")]
    [StringLength(20)]
    public string Birimi { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyatı 0'dan büyük olmalıdır")]
    public decimal BirimFiyati { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Mevcut adet negatif olamaz")]
    public decimal MevcutAdet { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum stok adedi negatif olamaz")]
    public decimal? MinimumStokAdedi { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }

    public bool IsActive { get; set; }
}
