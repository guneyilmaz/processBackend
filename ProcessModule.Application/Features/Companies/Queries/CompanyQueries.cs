using MediatR;
using ProcessModule.Application.Features.Companies.Commands;

namespace ProcessModule.Application.Features.Companies.Queries;

public class GetAllCompaniesQuery : IRequest<List<CompanyDto>>
{
    public bool ActiveOnly { get; set; } = false;
    public bool IncludeWorkplaces { get; set; } = false;
}

public class GetCompanyByIdQuery : IRequest<CompanyDto?>
{
    public int Id { get; set; }
    public bool IncludeWorkplaces { get; set; } = true;
}

public class GetCompaniesWithWorkplacesQuery : IRequest<List<CompanyWithWorkplacesDto>>
{
}