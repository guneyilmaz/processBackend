using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.CariHesap.Queries;

public class GetAllCariHesapQuery : IRequest<List<CariHesapDto>>
{
}

public class GetCariHesapByIdQuery : IRequest<CariHesapDto?>
{
    public int Id { get; set; }
}
