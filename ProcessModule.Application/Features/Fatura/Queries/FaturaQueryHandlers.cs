using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Fatura.Queries;

public class GetAllFaturaHandler : IRequestHandler<GetAllFaturaQuery, List<FaturaDto>>
{
    private readonly IFaturaRepository _faturaRepository;

    public GetAllFaturaHandler(IFaturaRepository faturaRepository)
    {
        _faturaRepository = faturaRepository;
    }

    public async Task<List<FaturaDto>> Handle(GetAllFaturaQuery request, CancellationToken cancellationToken)
    {
        var faturalar = await _faturaRepository.GetAllAsync();

        return faturalar.Select(f => new FaturaDto
        {
            Id = f.Id,
            FaturaNo = f.FaturaNo,
            FaturaTarihi = f.FaturaTarihi,
            CariHesapId = f.CariHesapId,
            CariAdi = f.CariHesap?.CariAdi,
            ToplamTutar = f.ToplamTutar,
            KdvTutari = f.KdvTutari,
            GenelToplam = f.GenelToplam,
            OncekiBakiye = f.OncekiBakiye,
            SonrakiBakiye = f.SonrakiBakiye,
            Aciklama = f.Aciklama,
            PdfPath = f.PdfPath,
            FaturaKalemleri = f.FaturaKalemleri.Select(fk => new FaturaKalemDto
            {
                Id = fk.Id,
                FaturaId = fk.FaturaId,
                StokId = fk.StokId,
                StokAdi = fk.Stok?.StokAdi,
                Birimi = fk.Stok?.Birimi,
                Miktar = fk.Miktar,
                BirimFiyat = fk.BirimFiyat,
                KdvOrani = fk.KdvOrani,
                AraToplam = fk.AraToplam,
                KdvTutari = fk.KdvTutari,
                Toplam = fk.Toplam
            }).ToList()
        }).ToList();
    }
}

public class GetFaturaByIdHandler : IRequestHandler<GetFaturaByIdQuery, FaturaDto?>
{
    private readonly IFaturaRepository _faturaRepository;

    public GetFaturaByIdHandler(IFaturaRepository faturaRepository)
    {
        _faturaRepository = faturaRepository;
    }

    public async Task<FaturaDto?> Handle(GetFaturaByIdQuery request, CancellationToken cancellationToken)
    {
        var fatura = await _faturaRepository.GetByIdWithDetailsAsync(request.Id);
        if (fatura == null)
            return null;

        return new FaturaDto
        {
            Id = fatura.Id,
            FaturaNo = fatura.FaturaNo,
            FaturaTarihi = fatura.FaturaTarihi,
            CariHesapId = fatura.CariHesapId,
            CariAdi = fatura.CariHesap?.CariAdi,
            ToplamTutar = fatura.ToplamTutar,
            KdvTutari = fatura.KdvTutari,
            GenelToplam = fatura.GenelToplam,
            OncekiBakiye = fatura.OncekiBakiye,
            SonrakiBakiye = fatura.SonrakiBakiye,
            Aciklama = fatura.Aciklama,
            PdfPath = fatura.PdfPath,
            FaturaKalemleri = fatura.FaturaKalemleri.Select(fk => new FaturaKalemDto
            {
                Id = fk.Id,
                FaturaId = fk.FaturaId,
                StokId = fk.StokId,
                StokAdi = fk.Stok?.StokAdi,
                Birimi = fk.Stok?.Birimi,
                Miktar = fk.Miktar,
                BirimFiyat = fk.BirimFiyat,
                KdvOrani = fk.KdvOrani,
                AraToplam = fk.AraToplam,
                KdvTutari = fk.KdvTutari,
                Toplam = fk.Toplam
            }).ToList()
        };
    }
}

public class GetFaturaByCariHesapHandler : IRequestHandler<GetFaturaByCariHesapQuery, List<FaturaDto>>
{
    private readonly IFaturaRepository _faturaRepository;

    public GetFaturaByCariHesapHandler(IFaturaRepository faturaRepository)
    {
        _faturaRepository = faturaRepository;
    }

    public async Task<List<FaturaDto>> Handle(GetFaturaByCariHesapQuery request, CancellationToken cancellationToken)
    {
        var faturalar = await _faturaRepository.GetByCariHesapIdAsync(request.CariHesapId);

        return faturalar.Select(f => new FaturaDto
        {
            Id = f.Id,
            FaturaNo = f.FaturaNo,
            FaturaTarihi = f.FaturaTarihi,
            CariHesapId = f.CariHesapId,
            CariAdi = f.CariHesap?.CariAdi,
            ToplamTutar = f.ToplamTutar,
            KdvTutari = f.KdvTutari,
            GenelToplam = f.GenelToplam,
            OncekiBakiye = f.OncekiBakiye,
            SonrakiBakiye = f.SonrakiBakiye,
            Aciklama = f.Aciklama,
            PdfPath = f.PdfPath,
            FaturaKalemleri = f.FaturaKalemleri.Select(fk => new FaturaKalemDto
            {
                Id = fk.Id,
                FaturaId = fk.FaturaId,
                StokId = fk.StokId,
                StokAdi = fk.Stok?.StokAdi,
                Birimi = fk.Stok?.Birimi,
                Miktar = fk.Miktar,
                BirimFiyat = fk.BirimFiyat,
                KdvOrani = fk.KdvOrani,
                AraToplam = fk.AraToplam,
                KdvTutari = fk.KdvTutari,
                Toplam = fk.Toplam
            }).ToList()
        }).ToList();
    }
}

public class GenerateNextFaturaNoHandler : IRequestHandler<GenerateNextFaturaNoQuery, string>
{
    private readonly IFaturaRepository _faturaRepository;

    public GenerateNextFaturaNoHandler(IFaturaRepository faturaRepository)
    {
        _faturaRepository = faturaRepository;
    }

    public async Task<string> Handle(GenerateNextFaturaNoQuery request, CancellationToken cancellationToken)
    {
        return await _faturaRepository.GenerateNextFaturaNoAsync();
    }
}
