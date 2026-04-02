using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Stok.Commands;

public class CreateStokHandler : IRequestHandler<CreateStokCommand, StokDto>
{
    private readonly IStokRepository _stokRepository;

    public CreateStokHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<StokDto> Handle(CreateStokCommand request, CancellationToken cancellationToken)
    {
        // Check if stok kodu already exists
        if (await _stokRepository.StokKoduExistsAsync(request.Stok.StokKodu))
        {
            throw new InvalidOperationException($"Stok kodu '{request.Stok.StokKodu}' zaten mevcut.");
        }

        var stok = new Domain.Entities.Stok
        {
            StokKodu = request.Stok.StokKodu,
            StokAdi = request.Stok.StokAdi,
            Birimi = request.Stok.Birimi,
            BirimFiyati = request.Stok.BirimFiyati,
            MevcutAdet = request.Stok.MevcutAdet,
            MinimumStokAdedi = request.Stok.MinimumStokAdedi,
            Aciklama = request.Stok.Aciklama,
            IsActive = true
        };

        var created = await _stokRepository.CreateAsync(stok);

        return new StokDto
        {
            Id = created.Id,
            StokKodu = created.StokKodu,
            StokAdi = created.StokAdi,
            Birimi = created.Birimi,
            BirimFiyati = created.BirimFiyati,
            MevcutAdet = created.MevcutAdet,
            MinimumStokAdedi = created.MinimumStokAdedi,
            Aciklama = created.Aciklama,
            IsActive = created.IsActive
        };
    }
}

public class UpdateStokHandler : IRequestHandler<UpdateStokCommand, StokDto?>
{
    private readonly IStokRepository _stokRepository;

    public UpdateStokHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<StokDto?> Handle(UpdateStokCommand request, CancellationToken cancellationToken)
    {
        var stok = await _stokRepository.GetByIdAsync(request.Stok.Id);
        if (stok == null)
            return null;

        // Check if stok kodu already exists (excluding current stok)
        if (await _stokRepository.StokKoduExistsAsync(request.Stok.StokKodu, request.Stok.Id))
        {
            throw new InvalidOperationException($"Stok kodu '{request.Stok.StokKodu}' zaten mevcut.");
        }

        stok.StokKodu = request.Stok.StokKodu;
        stok.StokAdi = request.Stok.StokAdi;
        stok.Birimi = request.Stok.Birimi;
        stok.BirimFiyati = request.Stok.BirimFiyati;
        stok.MevcutAdet = request.Stok.MevcutAdet;
        stok.MinimumStokAdedi = request.Stok.MinimumStokAdedi;
        stok.Aciklama = request.Stok.Aciklama;
        stok.IsActive = request.Stok.IsActive;

        var updated = await _stokRepository.UpdateAsync(stok);

        return new StokDto
        {
            Id = updated.Id,
            StokKodu = updated.StokKodu,
            StokAdi = updated.StokAdi,
            Birimi = updated.Birimi,
            BirimFiyati = updated.BirimFiyati,
            MevcutAdet = updated.MevcutAdet,
            MinimumStokAdedi = updated.MinimumStokAdedi,
            Aciklama = updated.Aciklama,
            IsActive = updated.IsActive
        };
    }
}

public class DeleteStokHandler : IRequestHandler<DeleteStokCommand, bool>
{
    private readonly IStokRepository _stokRepository;

    public DeleteStokHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<bool> Handle(DeleteStokCommand request, CancellationToken cancellationToken)
    {
        var stok = await _stokRepository.GetByIdAsync(request.Id);
        if (stok == null)
            return false;

        await _stokRepository.DeleteAsync(request.Id);
        return true;
    }
}
