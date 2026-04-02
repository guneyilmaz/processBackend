using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.CariHesap.Queries;

public class GetAllCariHesapHandler : IRequestHandler<GetAllCariHesapQuery, List<CariHesapDto>>
{
    private readonly ICariHesapRepository _cariHesapRepository;

    public GetAllCariHesapHandler(ICariHesapRepository cariHesapRepository)
    {
        _cariHesapRepository = cariHesapRepository;
    }

    public async Task<List<CariHesapDto>> Handle(GetAllCariHesapQuery request, CancellationToken cancellationToken)
    {
        var cariHesaplar = await _cariHesapRepository.GetAllAsync();

        return cariHesaplar.Select(c => new CariHesapDto
        {
            Id = c.Id,
            CariKodu = c.CariKodu,
            CariAdi = c.CariAdi,
            VergiNumarasi = c.VergiNumarasi,
            VergiDairesi = c.VergiDairesi,
            Adres = c.Adres,
            Telefon = c.Telefon,
            Email = c.Email,
            YetkiliKisi = c.YetkiliKisi,
            Bakiye = c.Bakiye,
            IsActive = c.IsActive,
            CompanyId = c.CompanyId,
            CompanyName = c.Company?.Name
        }).ToList();
    }
}

public class GetCariHesapByIdHandler : IRequestHandler<GetCariHesapByIdQuery, CariHesapDto?>
{
    private readonly ICariHesapRepository _cariHesapRepository;

    public GetCariHesapByIdHandler(ICariHesapRepository cariHesapRepository)
    {
        _cariHesapRepository = cariHesapRepository;
    }

    public async Task<CariHesapDto?> Handle(GetCariHesapByIdQuery request, CancellationToken cancellationToken)
    {
        var cariHesap = await _cariHesapRepository.GetByIdAsync(request.Id);
        if (cariHesap == null)
            return null;

        return new CariHesapDto
        {
            Id = cariHesap.Id,
            CariKodu = cariHesap.CariKodu,
            CariAdi = cariHesap.CariAdi,
            VergiNumarasi = cariHesap.VergiNumarasi,
            VergiDairesi = cariHesap.VergiDairesi,
            Adres = cariHesap.Adres,
            Telefon = cariHesap.Telefon,
            Email = cariHesap.Email,
            YetkiliKisi = cariHesap.YetkiliKisi,
            Bakiye = cariHesap.Bakiye,
            IsActive = cariHesap.IsActive,
            CompanyId = cariHesap.CompanyId,
            CompanyName = cariHesap.Company?.Name
        };
    }
}
