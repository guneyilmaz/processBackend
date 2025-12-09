using MediatR;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Features.Companies.Commands;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        // Validation checks
        if (await _companyRepository.IsCompanyNameExistsAsync(request.Name))
        {
            throw new ArgumentException("Bu firma adı zaten kullanılmaktadır.");
        }

        if (await _companyRepository.IsTaxNumberExistsAsync(request.TaxNumber))
        {
            throw new ArgumentException("Bu vergi numarası zaten kullanılmaktadır.");
        }

        var company = new Company
        {
            Name = request.Name,
            TaxNumber = request.TaxNumber,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            Website = request.Website,
            IsActive = request.IsActive
        };

        await _companyRepository.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();

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

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);
        if (company == null)
        {
            throw new ArgumentException("Firma bulunamadı.");
        }

        // Validation checks
        if (await _companyRepository.IsCompanyNameExistsAsync(request.Name, request.Id))
        {
            throw new ArgumentException("Bu firma adı zaten kullanılmaktadır.");
        }

        if (await _companyRepository.IsTaxNumberExistsAsync(request.TaxNumber, request.Id))
        {
            throw new ArgumentException("Bu vergi numarası zaten kullanılmaktadır.");
        }

        company.Name = request.Name;
        company.TaxNumber = request.TaxNumber;
        company.Address = request.Address;
        company.Phone = request.Phone;
        company.Email = request.Email;
        company.Website = request.Website;
        company.IsActive = request.IsActive;

        await _companyRepository.UpdateAsync(company);
        await _unitOfWork.SaveChangesAsync();

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