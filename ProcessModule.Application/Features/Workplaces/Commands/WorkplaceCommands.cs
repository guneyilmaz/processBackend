using System.ComponentModel.DataAnnotations;
using MediatR;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Workplaces.Commands;

public class CreateWorkplaceCommand : IRequest<WorkplaceDto>
{
    [Required(ErrorMessage = "Firma ID zorunludur.")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "İşyeri adı zorunludur.")]
    [StringLength(100, ErrorMessage = "İşyeri adı en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
    public string? Address { get; set; }

    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir.")]
    public string? Phone { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateWorkplaceCommand : IRequest<WorkplaceDto>
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Firma ID zorunludur.")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "İşyeri adı zorunludur.")]
    [StringLength(100, ErrorMessage = "İşyeri adı en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
    public string? Address { get; set; }

    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir.")]
    public string? Phone { get; set; }

    public bool IsActive { get; set; } = true;
}