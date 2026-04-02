using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProcessModule.Application.Services;

public class FaturaPdfService : IFaturaPdfService
{
    public byte[] GenerateFaturaPdf(Fatura fatura)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header()
                    .Text($"FATURA")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        // Fatura Başlık Bilgileri
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Fatura No: {fatura.FaturaNo}").Bold();
                                col.Item().Text($"Tarih: {fatura.FaturaTarihi:dd.MM.yyyy}");
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Cari Hesap: {fatura.CariHesap?.CariAdi ?? "Bilinmiyor"}").Bold();
                                if (!string.IsNullOrEmpty(fatura.CariHesap?.VergiDairesi))
                                {
                                    col.Item().Text($"V.D.: {fatura.CariHesap.VergiDairesi}");
                                }
                                if (!string.IsNullOrEmpty(fatura.CariHesap?.VergiNumarasi))
                                {
                                    col.Item().Text($"V.N.: {fatura.CariHesap.VergiNumarasi}");
                                }
                                if (!string.IsNullOrEmpty(fatura.CariHesap?.Adres))
                                {
                                    col.Item().Text($"Adres: {fatura.CariHesap.Adres}");
                                }
                            });
                        });

                        // Fatura Kalemleri Tablosu
                        column.Item().PaddingTop(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);  // Sıra
                                columns.RelativeColumn(3);   // Stok
                                columns.ConstantColumn(60);  // Miktar
                                columns.ConstantColumn(80);  // Birim Fiyat
                                columns.ConstantColumn(50);  // KDV %
                                columns.ConstantColumn(80);  // Toplam
                            });

                            // Başlık
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#").Bold();
                                header.Cell().Element(CellStyle).Text("Stok Adı").Bold();
                                header.Cell().Element(CellStyle).Text("Miktar").Bold();
                                header.Cell().Element(CellStyle).Text("Birim Fiyat").Bold();
                                header.Cell().Element(CellStyle).Text("KDV %").Bold();
                                header.Cell().Element(CellStyle).Text("Toplam").Bold();
                            });

                            // Kalemler
                            int sira = 1;
                            if (fatura.FaturaKalemleri != null)
                            {
                                foreach (var kalem in fatura.FaturaKalemleri)
                                {
                                    table.Cell().Element(CellStyle).Text(sira++.ToString());
                                    table.Cell().Element(CellStyle).Text(kalem.Stok?.StokAdi ?? "Bilinmiyor");
                                    table.Cell().Element(CellStyle).Text($"{kalem.Miktar:N2}");
                                    table.Cell().Element(CellStyle).Text($"{kalem.BirimFiyat:C2}");
                                    table.Cell().Element(CellStyle).Text($"%{kalem.KdvOrani:N0}");
                                    table.Cell().Element(CellStyle).Text($"{kalem.Toplam:C2}");
                                }
                            }
                        });

                        // Toplam Bilgileri
                        column.Item().PaddingTop(20).AlignRight().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Ara Toplam:").Bold();
                                row.ConstantItem(100).Text($"{fatura.ToplamTutar:C2}");
                            });

                            col.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("KDV:").Bold();
                                row.ConstantItem(100).Text($"{fatura.KdvTutari:C2}");
                            });

                            col.Item().BorderTop(1).BorderColor(Colors.Grey.Darken2).PaddingTop(5).Row(row =>
                            {
                                row.ConstantItem(150).Text("GENEL TOPLAM:").Bold().FontSize(12);
                                row.ConstantItem(100).Text($"{fatura.GenelToplam:C2}").Bold().FontSize(12);
                            });
                        });

                        // Bakiye Bilgileri
                        if (fatura.OncekiBakiye != 0 || fatura.SonrakiBakiye != 0)
                        {
                            column.Item().PaddingTop(20).BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10).Column(col =>
                            {
                                col.Item().Text("Bakiye Bilgileri").Bold().FontSize(11);
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(150).Text("Önceki Bakiye:");
                                    row.ConstantItem(100).Text($"{fatura.OncekiBakiye:C2}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(150).Text("Sonraki Bakiye:");
                                    row.ConstantItem(100).Text($"{fatura.SonrakiBakiye:C2}");
                                });
                            });
                        }

                        // Açıklama
                        if (!string.IsNullOrEmpty(fatura.Aciklama))
                        {
                            column.Item().PaddingTop(20).Column(col =>
                            {
                                col.Item().Text("Açıklama:").Bold();
                                col.Item().Text(fatura.Aciklama);
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten3)
            .Padding(5);
    }
}
