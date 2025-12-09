using MediatR;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Workplaces.Commands;

public class CreateWorkplaceHandler : IRequestHandler<CreateWorkplaceCommand, WorkplaceDto>
{
    private readonly IWorkplaceRepository _workplaceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkplaceHandler(IWorkplaceRepository workplaceRepository, ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _workplaceRepository = workplaceRepository;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WorkplaceDto> Handle(CreateWorkplaceCommand request, CancellationToken cancellationToken)
    {
        // Company var mı kontrol et
        var company = await _companyRepository.GetByIdAsync(request.CompanyId);
        if (company == null)
        {
            throw new ArgumentException("Firma bulunamadı.");
        }

        var workplace = new Workplace
        {
            CompanyId = request.CompanyId,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            IsActive = request.IsActive
        };

        await _workplaceRepository.AddAsync(workplace);
        await _unitOfWork.SaveChangesAsync();

        return new WorkplaceDto
        {
            Id = workplace.Id,
            CompanyId = workplace.CompanyId,
            Name = workplace.Name,
            Address = workplace.Address,
            Phone = workplace.Phone,
            Email = null, // Workplace entity'sinde Email yok
            IsActive = workplace.IsActive,
            CreatedAt = workplace.CreatedAt,
            UpdatedAt = workplace.UpdatedAt
        };
    }
}

public class UpdateWorkplaceHandler : IRequestHandler<UpdateWorkplaceCommand, WorkplaceDto>
{
    private readonly IWorkplaceRepository _workplaceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkplaceHandler(IWorkplaceRepository workplaceRepository, ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _workplaceRepository = workplaceRepository;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WorkplaceDto> Handle(UpdateWorkplaceCommand request, CancellationToken cancellationToken)
    {
        var workplace = await _workplaceRepository.GetByIdAsync(request.Id);
        if (workplace == null)
        {
            throw new ArgumentException("İşyeri bulunamadı.");
        }

        // Company var mı kontrol et
        var company = await _companyRepository.GetByIdAsync(request.CompanyId);
        if (company == null)
        {
            throw new ArgumentException("Firma bulunamadı.");
        }

        workplace.CompanyId = request.CompanyId;
        workplace.Name = request.Name;
        workplace.Address = request.Address;
        workplace.Phone = request.Phone;
        workplace.IsActive = request.IsActive;

        await _workplaceRepository.UpdateAsync(workplace);
        await _unitOfWork.SaveChangesAsync();

        return new WorkplaceDto
        {
            Id = workplace.Id,
            CompanyId = workplace.CompanyId,
            Name = workplace.Name,
            Address = workplace.Address,
            Phone = workplace.Phone,
            Email = null, // Workplace entity'sinde Email yok
            IsActive = workplace.IsActive,
            CreatedAt = workplace.CreatedAt,
            UpdatedAt = workplace.UpdatedAt
        };
    }
}