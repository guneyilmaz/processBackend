using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Stok.Commands;

public class CreateStokCommand : IRequest<StokDto>
{
    public CreateStokDto Stok { get; set; } = null!;
}

public class UpdateStokCommand : IRequest<StokDto?>
{
    public UpdateStokDto Stok { get; set; } = null!;
}

public class DeleteStokCommand : IRequest<bool>
{
    public int Id { get; set; }
}
