using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Fatura.Queries;

public class GetAllFaturaQuery : IRequest<List<FaturaDto>>
{
}

public class GetFaturaByIdQuery : IRequest<FaturaDto?>
{
    public int Id { get; set; }
}

public class GetFaturaByCariHesapQuery : IRequest<List<FaturaDto>>
{
    public int CariHesapId { get; set; }
}

public class GenerateNextFaturaNoQuery : IRequest<string>
{
}
