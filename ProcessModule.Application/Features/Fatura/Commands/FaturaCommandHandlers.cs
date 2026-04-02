using MediatR;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Features.Fatura.Commands;

public class CreateFaturaHandler : IRequestHandler<CreateFaturaCommand, FaturaDto>
{
    private readonly IFaturaRepository _faturaRepository;
    private readonly ICariHesapRepository _cariHesapRepository;
    private readonly IStokRepository _stokRepository;

    public CreateFaturaHandler(
        IFaturaRepository faturaRepository,
        ICariHesapRepository cariHesapRepository,
        IStokRepository stokRepository)
    {
        _faturaRepository = faturaRepository;
        _cariHesapRepository = cariHesapRepository;
        _stokRepository = stokRepository;
    }

    public async Task<FaturaDto> Handle(CreateFaturaCommand request, CancellationToken cancellationToken)
    {
        // Check if fatura no already exists
        if (await _faturaRepository.FaturaNoExistsAsync(request.Fatura.FaturaNo))
        {
            throw new InvalidOperationException($"Fatura no '{request.Fatura.FaturaNo}' zaten mevcut.");
        }

        // Get cari hesap
        var cariHesap = await _cariHesapRepository.GetByIdAsync(request.Fatura.CariHesapId);
        if (cariHesap == null)
        {
            throw new InvalidOperationException("Cari hesap bulunamadı.");
        }

        // Create fatura kalemleri
        var faturaKalemleri = new List<FaturaKalem>();
        decimal toplamTutar = 0;
        decimal kdvTutari = 0;

        foreach (var kalemDto in request.Fatura.FaturaKalemleri)
        {
            // Validate stok
            var stok = await _stokRepository.GetByIdAsync(kalemDto.StokId);
            if (stok == null)
            {
                throw new InvalidOperationException($"Stok bulunamadı: {kalemDto.StokId}");
            }

            // Check stok availability
            if (stok.MevcutAdet < kalemDto.Miktar)
            {
                throw new InvalidOperationException($"Yetersiz stok: {stok.StokAdi}. Mevcut: {stok.MevcutAdet}, İstenen: {kalemDto.Miktar}");
            }

            // Calculate kalem totals
            var araToplam = kalemDto.Miktar * kalemDto.BirimFiyat;
            var kalemKdvTutari = araToplam * kalemDto.KdvOrani / 100;
            var kalemToplam = araToplam + kalemKdvTutari;

            var faturaKalem = new FaturaKalem
            {
                StokId = kalemDto.StokId,
                Miktar = kalemDto.Miktar,
                BirimFiyat = kalemDto.BirimFiyat,
                KdvOrani = kalemDto.KdvOrani,
                AraToplam = araToplam,
                KdvTutari = kalemKdvTutari,
                Toplam = kalemToplam
            };

            faturaKalemleri.Add(faturaKalem);
            toplamTutar += araToplam;
            kdvTutari += kalemKdvTutari;

            // Update stok quantity
            stok.MevcutAdet -= kalemDto.Miktar;
            await _stokRepository.UpdateAsync(stok);
        }

        var genelToplam = toplamTutar + kdvTutari;

        // Create fatura
        var fatura = new Domain.Entities.Fatura
        {
            FaturaNo = request.Fatura.FaturaNo,
            FaturaTarihi = request.Fatura.FaturaTarihi,
            CariHesapId = request.Fatura.CariHesapId,
            ToplamTutar = toplamTutar,
            KdvTutari = kdvTutari,
            GenelToplam = genelToplam,
            OncekiBakiye = cariHesap.Bakiye,
            SonrakiBakiye = cariHesap.Bakiye + genelToplam,
            Aciklama = request.Fatura.Aciklama,
            FaturaKalemleri = faturaKalemleri
        };

        var created = await _faturaRepository.CreateAsync(fatura);

        // Update cari hesap bakiye
        await _cariHesapRepository.UpdateBakiyeAsync(request.Fatura.CariHesapId, genelToplam);

        // Load details for response
        var result = await _faturaRepository.GetByIdWithDetailsAsync(created.Id);

        return new FaturaDto
        {
            Id = result!.Id,
            FaturaNo = result.FaturaNo,
            FaturaTarihi = result.FaturaTarihi,
            CariHesapId = result.CariHesapId,
            CariAdi = result.CariHesap?.CariAdi,
            ToplamTutar = result.ToplamTutar,
            KdvTutari = result.KdvTutari,
            GenelToplam = result.GenelToplam,
            OncekiBakiye = result.OncekiBakiye,
            SonrakiBakiye = result.SonrakiBakiye,
            Aciklama = result.Aciklama,
            PdfPath = result.PdfPath,
            FaturaKalemleri = result.FaturaKalemleri.Select(fk => new FaturaKalemDto
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

public class UpdateFaturaHandler : IRequestHandler<UpdateFaturaCommand, FaturaDto?>
{
    private readonly IFaturaRepository _faturaRepository;

    public UpdateFaturaHandler(IFaturaRepository faturaRepository)
    {
        _faturaRepository = faturaRepository;
    }

    public async Task<FaturaDto?> Handle(UpdateFaturaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _faturaRepository.GetByIdAsync(request.Fatura.Id);
        if (fatura == null)
            return null;

        // Note: Only allowing basic info update, not kalemler
        fatura.FaturaNo = request.Fatura.FaturaNo;
        fatura.FaturaTarihi = request.Fatura.FaturaTarihi;
        fatura.Aciklama = request.Fatura.Aciklama;

        var updated = await _faturaRepository.UpdateAsync(fatura);
        var result = await _faturaRepository.GetByIdWithDetailsAsync(updated.Id);

        return new FaturaDto
        {
            Id = result!.Id,
            FaturaNo = result.FaturaNo,
            FaturaTarihi = result.FaturaTarihi,
            CariHesapId = result.CariHesapId,
            CariAdi = result.CariHesap?.CariAdi,
            ToplamTutar = result.ToplamTutar,
            KdvTutari = result.KdvTutari,
            GenelToplam = result.GenelToplam,
            OncekiBakiye = result.OncekiBakiye,
            SonrakiBakiye = result.SonrakiBakiye,
            Aciklama = result.Aciklama,
            PdfPath = result.PdfPath,
            FaturaKalemleri = result.FaturaKalemleri.Select(fk => new FaturaKalemDto
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

public class DeleteFaturaHandler : IRequestHandler<DeleteFaturaCommand, bool>
{
    private readonly IFaturaRepository _faturaRepository;
    private readonly ICariHesapRepository _cariHesapRepository;
    private readonly IStokRepository _stokRepository;

    public DeleteFaturaHandler(
        IFaturaRepository faturaRepository,
        ICariHesapRepository cariHesapRepository,
        IStokRepository stokRepository)
    {
        _faturaRepository = faturaRepository;
        _cariHesapRepository = cariHesapRepository;
        _stokRepository = stokRepository;
    }

    public async Task<bool> Handle(DeleteFaturaCommand request, CancellationToken cancellationToken)
    {
        var fatura = await _faturaRepository.GetByIdWithDetailsAsync(request.Id);
        if (fatura == null)
            return false;

        // Reverse stok quantities
        foreach (var kalem in fatura.FaturaKalemleri)
        {
            var stok = await _stokRepository.GetByIdAsync(kalem.StokId);
            if (stok != null)
            {
                stok.MevcutAdet += kalem.Miktar;
                await _stokRepository.UpdateAsync(stok);
            }
        }

        // Reverse cari bakiye
        await _cariHesapRepository.UpdateBakiyeAsync(fatura.CariHesapId, -fatura.GenelToplam);

        // Delete fatura
        await _faturaRepository.DeleteAsync(request.Id);
        return true;
    }
}
