using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ProcessModule.Application.Features.Companies.Commands;

public class CreateCompanyCommand : IRequest<CompanyDto>
{
    [Required(ErrorMessage = "Firma adı zorunludur")]
    [StringLength(200, ErrorMessage = "Firma adı en fazla 200 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vergi numarası zorunludur")]
    [StringLength(20, ErrorMessage = "Vergi numarası en fazla 20 karakter olabilir")]
    public string TaxNumber { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    public string? Address { get; set; }

    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    [StringLength(255, ErrorMessage = "Email en fazla 255 karakter olabilir")]
    public string? Email { get; set; }

    [Url(ErrorMessage = "Geçerli bir website adresi giriniz")]
    [StringLength(255, ErrorMessage = "Website en fazla 255 karakter olabilir")]
    public string? Website { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateCompanyCommand : IRequest<CompanyDto>
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Firma adı zorunludur")]
    [StringLength(200, ErrorMessage = "Firma adı en fazla 200 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vergi numarası zorunludur")]
    [StringLength(20, ErrorMessage = "Vergi numarası en fazla 20 karakter olabilir")]
    public string TaxNumber { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    public string? Address { get; set; }

    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    [StringLength(255, ErrorMessage = "Email en fazla 255 karakter olabilir")]
    public string? Email { get; set; }

    [Url(ErrorMessage = "Geçerli bir website adresi giriniz")]
    [StringLength(255, ErrorMessage = "Website en fazla 255 karakter olabilir")]
    public string? Website { get; set; }

    public bool IsActive { get; set; } = true;
}

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CompanyWithWorkplacesDto : CompanyDto
{
    public List<WorkplaceDto> Workplaces { get; set; } = new List<WorkplaceDto>();
}

public class WorkplaceDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}