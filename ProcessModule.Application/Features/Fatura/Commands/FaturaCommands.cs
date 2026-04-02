using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Fatura.Commands;

public class CreateFaturaCommand : IRequest<FaturaDto>
{
    public CreateFaturaDto Fatura { get; set; } = null!;
}

public class UpdateFaturaCommand : IRequest<FaturaDto?>
{
    public UpdateFaturaDto Fatura { get; set; } = null!;
}

public class DeleteFaturaCommand : IRequest<bool>
{
    public int Id { get; set; }
}
