using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Stok.Queries;

public class GetAllStokHandler : IRequestHandler<GetAllStokQuery, List<StokDto>>
{
    private readonly IStokRepository _stokRepository;

    public GetAllStokHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<List<StokDto>> Handle(GetAllStokQuery request, CancellationToken cancellationToken)
    {
        var stoklar = await _stokRepository.GetAllAsync();

        return stoklar.Select(s => new StokDto
        {
            Id = s.Id,
            StokKodu = s.StokKodu,
            StokAdi = s.StokAdi,
            Birimi = s.Birimi,
            BirimFiyati = s.BirimFiyati,
            MevcutAdet = s.MevcutAdet,
            MinimumStokAdedi = s.MinimumStokAdedi,
            Aciklama = s.Aciklama,
            IsActive = s.IsActive
        }).ToList();
    }
}

public class GetStokByIdHandler : IRequestHandler<GetStokByIdQuery, StokDto?>
{
    private readonly IStokRepository _stokRepository;

    public GetStokByIdHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<StokDto?> Handle(GetStokByIdQuery request, CancellationToken cancellationToken)
    {
        var stok = await _stokRepository.GetByIdAsync(request.Id);
        if (stok == null)
            return null;

        return new StokDto
        {
            Id = stok.Id,
            StokKodu = stok.StokKodu,
            StokAdi = stok.StokAdi,
            Birimi = stok.Birimi,
            BirimFiyati = stok.BirimFiyati,
            MevcutAdet = stok.MevcutAdet,
            MinimumStokAdedi = stok.MinimumStokAdedi,
            Aciklama = stok.Aciklama,
            IsActive = stok.IsActive
        };
    }
}

public class GetLowStockItemsHandler : IRequestHandler<GetLowStockItemsQuery, List<StokDto>>
{
    private readonly IStokRepository _stokRepository;

    public GetLowStockItemsHandler(IStokRepository stokRepository)
    {
        _stokRepository = stokRepository;
    }

    public async Task<List<StokDto>> Handle(GetLowStockItemsQuery request, CancellationToken cancellationToken)
    {
        var stoklar = await _stokRepository.GetLowStockItemsAsync();

        return stoklar.Select(s => new StokDto
        {
            Id = s.Id,
            StokKodu = s.StokKodu,
            StokAdi = s.StokAdi,
            Birimi = s.Birimi,
            BirimFiyati = s.BirimFiyati,
            MevcutAdet = s.MevcutAdet,
            MinimumStokAdedi = s.MinimumStokAdedi,
            Aciklama = s.Aciklama,
            IsActive = s.IsActive
        }).ToList();
    }
}
