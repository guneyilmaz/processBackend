using MediatR;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Workplaces.Queries;

public class GetAllWorkplacesQuery : IRequest<List<WorkplaceDto>>
{
}

public class GetWorkplaceByIdQuery : IRequest<WorkplaceDto?>
{
    public int Id { get; set; }
}

public class GetWorkplacesByCompanyIdQuery : IRequest<List<WorkplaceDto>>
{
    public int CompanyId { get; set; }
}