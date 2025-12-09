using MediatR;
using ProcessModule.Application.Interfaces;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Companies.Queries;

public class GetAllCompaniesHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync();

        return companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            Name = c.Name,
            TaxNumber = c.TaxNumber,
            Address = c.Address,
            Phone = c.Phone,
            Email = c.Email,
            Website = c.Website,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }
}

public class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto?>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByIdHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyDto?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);
        if (company == null)
            return null;

        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            TaxNumber = company.TaxNumber,
            Address = company.Address,
            Phone = company.Phone,
            Email = company.Email,
            Website = company.Website,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt
        };
    }
}

public class GetCompaniesWithWorkplacesHandler : IRequestHandler<GetCompaniesWithWorkplacesQuery, List<CompanyWithWorkplacesDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompaniesWithWorkplacesHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<CompanyWithWorkplacesDto>> Handle(GetCompaniesWithWorkplacesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetCompaniesWithWorkplacesAsync();

        return companies.Select(c => new CompanyWithWorkplacesDto
        {
            Id = c.Id,
            Name = c.Name,
            TaxNumber = c.TaxNumber,
            Address = c.Address,
            Phone = c.Phone,
            Email = c.Email,
            Website = c.Website,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            Workplaces = c.Workplaces.Select(w => new WorkplaceDto
            {
                Id = w.Id,
                CompanyId = w.CompanyId,
                Name = w.Name,
                Address = w.Address,
                Phone = w.Phone,
                Email = null, // Workplace entity'sinde Email yok
                IsActive = w.IsActive,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            }).ToList()
        }).ToList();
    }
}