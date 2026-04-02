using MediatR;
using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.CariHesap.Commands;

public class CreateCariHesapCommand : IRequest<CariHesapDto>
{
    public CreateCariHesapDto CariHesap { get; set; } = null!;
}

public class UpdateCariHesapCommand : IRequest<CariHesapDto?>
{
    public UpdateCariHesapDto CariHesap { get; set; } = null!;
}

public class DeleteCariHesapCommand : IRequest<bool>
{
    public int Id { get; set; }
}
