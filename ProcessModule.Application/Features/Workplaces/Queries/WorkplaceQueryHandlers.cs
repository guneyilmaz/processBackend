using MediatR;
using ProcessModule.Application.Interfaces;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Workplaces.Queries;

public class GetAllWorkplacesHandler : IRequestHandler<GetAllWorkplacesQuery, List<WorkplaceDto>>
{
    private readonly IWorkplaceRepository _workplaceRepository;

    public GetAllWorkplacesHandler(IWorkplaceRepository workplaceRepository)
    {
        _workplaceRepository = workplaceRepository;
    }

    public async Task<List<WorkplaceDto>> Handle(GetAllWorkplacesQuery request, CancellationToken cancellationToken)
    {
        var workplaces = await _workplaceRepository.GetAllAsync();

        return workplaces.Select(w => new WorkplaceDto
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
        }).ToList();
    }
}

public class GetWorkplaceByIdHandler : IRequestHandler<GetWorkplaceByIdQuery, WorkplaceDto?>
{
    private readonly IWorkplaceRepository _workplaceRepository;

    public GetWorkplaceByIdHandler(IWorkplaceRepository workplaceRepository)
    {
        _workplaceRepository = workplaceRepository;
    }

    public async Task<WorkplaceDto?> Handle(GetWorkplaceByIdQuery request, CancellationToken cancellationToken)
    {
        var workplace = await _workplaceRepository.GetByIdAsync(request.Id);
        if (workplace == null)
            return null;

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

public class GetWorkplacesByCompanyIdHandler : IRequestHandler<GetWorkplacesByCompanyIdQuery, List<WorkplaceDto>>
{
    private readonly IWorkplaceRepository _workplaceRepository;

    public GetWorkplacesByCompanyIdHandler(IWorkplaceRepository workplaceRepository)
    {
        _workplaceRepository = workplaceRepository;
    }

    public async Task<List<WorkplaceDto>> Handle(GetWorkplacesByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var workplaces = await _workplaceRepository.GetWorkplacesByCompanyAsync(request.CompanyId);

        return workplaces.Select(w => new WorkplaceDto
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
        }).ToList();
    }
}