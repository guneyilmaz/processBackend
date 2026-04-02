using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Interfaces;

public interface IFaturaPdfService
{
    byte[] GenerateFaturaPdf(Fatura fatura);
}
