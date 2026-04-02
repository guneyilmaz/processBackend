using System.ComponentModel.DataAnnotations;

namespace ProcessModule.Application.DTOs;

// CariHesap DTOs
public class CariHesapDto
{
    public int Id { get; set; }
    public string CariKodu { get; set; } = string.Empty;
    public string CariAdi { get; set; } = string.Empty;
    public string? VergiNumarasi { get; set; }
    public string? VergiDairesi { get; set; }
    public string? Adres { get; set; }
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public string? YetkiliKisi { get; set; }
    public decimal Bakiye { get; set; }
    public bool IsActive { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }
}

public class CreateCariHesapDto
{
    [Required(ErrorMessage = "Cari kodu zorunludur")]
    [StringLength(50)]
    public string CariKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cari adı zorunludur")]
    [StringLength(200)]
    public string CariAdi { get; set; } = string.Empty;

    [StringLength(20)]
    public string? VergiNumarasi { get; set; }

    [StringLength(100)]
    public string? VergiDairesi { get; set; }

    [StringLength(500)]
    public string? Adres { get; set; }

    [StringLength(20)]
    public string? Telefon { get; set; }

    [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? YetkiliKisi { get; set; }

    public int? CompanyId { get; set; }
}

public class UpdateCariHesapDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Cari kodu zorunludur")]
    [StringLength(50)]
    public string CariKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cari adı zorunludur")]
    [StringLength(200)]
    public string CariAdi { get; set; } = string.Empty;

    [StringLength(20)]
    public string? VergiNumarasi { get; set; }

    [StringLength(100)]
    public string? VergiDairesi { get; set; }

    [StringLength(500)]
    public string? Adres { get; set; }

    [StringLength(20)]
    public string? Telefon { get; set; }

    [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? YetkiliKisi { get; set; }

    public bool IsActive { get; set; }

    public int? CompanyId { get; set; }
}
