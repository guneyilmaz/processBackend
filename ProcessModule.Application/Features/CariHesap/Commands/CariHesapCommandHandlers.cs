using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.CariHesap.Commands;

public class CreateCariHesapHandler : IRequestHandler<CreateCariHesapCommand, CariHesapDto>
{
    private readonly ICariHesapRepository _cariHesapRepository;

    public CreateCariHesapHandler(ICariHesapRepository cariHesapRepository)
    {
        _cariHesapRepository = cariHesapRepository;
    }

    public async Task<CariHesapDto> Handle(CreateCariHesapCommand request, CancellationToken cancellationToken)
    {
        // Check if cari kodu already exists
        if (await _cariHesapRepository.CariKoduExistsAsync(request.CariHesap.CariKodu))
        {
            throw new InvalidOperationException($"Cari kodu '{request.CariHesap.CariKodu}' zaten mevcut.");
        }

        var cariHesap = new Domain.Entities.CariHesap
        {
            CariKodu = request.CariHesap.CariKodu,
            CariAdi = request.CariHesap.CariAdi,
            VergiNumarasi = request.CariHesap.VergiNumarasi,
            VergiDairesi = request.CariHesap.VergiDairesi,
            Adres = request.CariHesap.Adres,
            Telefon = request.CariHesap.Telefon,
            Email = request.CariHesap.Email,
            YetkiliKisi = request.CariHesap.YetkiliKisi,
            CompanyId = request.CariHesap.CompanyId,
            IsActive = true
        };

        var created = await _cariHesapRepository.CreateAsync(cariHesap);

        return new CariHesapDto
        {
            Id = created.Id,
            CariKodu = created.CariKodu,
            CariAdi = created.CariAdi,
            VergiNumarasi = created.VergiNumarasi,
            VergiDairesi = created.VergiDairesi,
            Adres = created.Adres,
            Telefon = created.Telefon,
            Email = created.Email,
            YetkiliKisi = created.YetkiliKisi,
            Bakiye = created.Bakiye,
            IsActive = created.IsActive,
            CompanyId = created.CompanyId
        };
    }
}

public class UpdateCariHesapHandler : IRequestHandler<UpdateCariHesapCommand, CariHesapDto?>
{
    private readonly ICariHesapRepository _cariHesapRepository;

    public UpdateCariHesapHandler(ICariHesapRepository cariHesapRepository)
    {
        _cariHesapRepository = cariHesapRepository;
    }

    public async Task<CariHesapDto?> Handle(UpdateCariHesapCommand request, CancellationToken cancellationToken)
    {
        var cariHesap = await _cariHesapRepository.GetByIdAsync(request.CariHesap.Id);
        if (cariHesap == null)
            return null;

        // Check if cari kodu already exists (excluding current cariHesap)
        if (await _cariHesapRepository.CariKoduExistsAsync(request.CariHesap.CariKodu, request.CariHesap.Id))
        {
            throw new InvalidOperationException($"Cari kodu '{request.CariHesap.CariKodu}' zaten mevcut.");
        }

        cariHesap.CariKodu = request.CariHesap.CariKodu;
        cariHesap.CariAdi = request.CariHesap.CariAdi;
        cariHesap.VergiNumarasi = request.CariHesap.VergiNumarasi;
        cariHesap.VergiDairesi = request.CariHesap.VergiDairesi;
        cariHesap.Adres = request.CariHesap.Adres;
        cariHesap.Telefon = request.CariHesap.Telefon;
        cariHesap.Email = request.CariHesap.Email;
        cariHesap.YetkiliKisi = request.CariHesap.YetkiliKisi;
        cariHesap.IsActive = request.CariHesap.IsActive;
        cariHesap.CompanyId = request.CariHesap.CompanyId;

        var updated = await _cariHesapRepository.UpdateAsync(cariHesap);

        return new CariHesapDto
        {
            Id = updated.Id,
            CariKodu = updated.CariKodu,
            CariAdi = updated.CariAdi,
            VergiNumarasi = updated.VergiNumarasi,
            VergiDairesi = updated.VergiDairesi,
            Adres = updated.Adres,
            Telefon = updated.Telefon,
            Email = updated.Email,
            YetkiliKisi = updated.YetkiliKisi,
            Bakiye = updated.Bakiye,
            IsActive = updated.IsActive,
            CompanyId = updated.CompanyId
        };
    }
}

public class DeleteCariHesapHandler : IRequestHandler<DeleteCariHesapCommand, bool>
{
    private readonly ICariHesapRepository _cariHesapRepository;

    public DeleteCariHesapHandler(ICariHesapRepository cariHesapRepository)
    {
        _cariHesapRepository = cariHesapRepository;
    }

    public async Task<bool> Handle(DeleteCariHesapCommand request, CancellationToken cancellationToken)
    {
        var cariHesap = await _cariHesapRepository.GetByIdAsync(request.Id);
        if (cariHesap == null)
            return false;

        await _cariHesapRepository.DeleteAsync(request.Id);
        return true;
    }
}
