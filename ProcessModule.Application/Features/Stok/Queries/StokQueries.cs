using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Stok.Queries;

public class GetAllStokQuery : IRequest<List<StokDto>>
{
}

public class GetStokByIdQuery : IRequest<StokDto?>
{
    public int Id { get; set; }
}

public class GetLowStockItemsQuery : IRequest<List<StokDto>>
{
}
