# Stok, Cari Hesap ve Fatura Yönetim Modülü

## 📋 Genel Bakış

Bu geliştirmede, ProccesModule projesine üç yeni ana modül eklenmiştir:
1. **Stok Yönetimi** - Ürün stok takibi
2. **Cari Hesap Yönetimi** - Müşteri hesap yönetimi
3. **Fatura Yönetimi** - Fatura kesme ve takip

## 🗄️ Database (Backend)

### Yeni Tablolar
- **Stoklar** - Stok bilgileri
- **CariHesaplar** - Müşteri/cari bilgileri
- **Faturalar** - Fatura başlık bilgileri
- **FaturaKalemleri** - Fatura detay kalemleri

### Migration
```bash
cd c:\ProccesModuleBackend
dotnet ef database update -p ProcessModule.Persistence -s ProcessModule.WebAPI
```

Migration adı: `20260401075157_AddStokCariHesapFaturaTables`

## 🔧 Backend API

### Dosya Yapısı

#### Entities (Domain Layer)
- `ProcessModule.Domain/Entities/Stok.cs`
- `ProcessModule.Domain/Entities/CariHesap.cs`
- `ProcessModule.Domain/Entities/Fatura.cs`
- `ProcessModule.Domain/Entities/FaturaKalem.cs`

#### DTOs (Application Layer)
- `ProcessModule.Application/DTOs/StokDTOs.cs`
- `ProcessModule.Application/DTOs/CariHesapDTOs.cs`
- `ProcessModule.Application/DTOs/FaturaDTOs.cs`

#### Repositories
- `ProcessModule.Infrastructure/Repositories/StokRepository.cs`
- `ProcessModule.Infrastructure/Repositories/CariHesapRepository.cs`
- `ProcessModule.Infrastructure/Repositories/FaturaRepository.cs`

#### CQRS Handlers
- `ProcessModule.Application/Features/Stok/`
  - Commands/Queries/Handlers
- `ProcessModule.Application/Features/CariHesap/`
  - Commands/Queries/Handlers
- `ProcessModule.Application/Features/Fatura/`
  - Commands/Queries/Handlers

#### Controllers
- `ProcessModule.WebAPI/Controllers/StokController.cs`
- `ProcessModule.WebAPI/Controllers/CariHesapController.cs`
- `ProcessModule.WebAPI/Controllers/FaturaController.cs`

### API Endpoints

#### Stok API
- `GET /api/stok` - Tüm stokları listele
- `GET /api/stok/{id}` - Stok detayı
- `GET /api/stok/low-stock` - Düşük stok uyarıları
- `POST /api/stok` - Yeni stok ekle
- `PUT /api/stok/{id}` - Stok güncelle
- `DELETE /api/stok/{id}` - Stok sil

#### Cari Hesap API
- `GET /api/carihesap` - Tüm cari hesapları listele
- `GET /api/carihesap/{id}` - Cari hesap detayı
- `POST /api/carihesap` - Yeni cari hesap ekle
- `PUT /api/carihesap/{id}` - Cari hesap güncelle
- `DELETE /api/carihesap/{id}` - Cari hesap sil

#### Fatura API
- `GET /api/fatura` - Tüm faturaları listele
- `GET /api/fatura/{id}` - Fatura detayı
- `GET /api/fatura/cari/{cariHesapId}` - Cariye ait faturalar
- `GET /api/fatura/generate-fatura-no` - Sıradaki fatura no
- `POST /api/fatura` - Yeni fatura kes
- `PUT /api/fatura/{id}` - Fatura güncelle
- `DELETE /api/fatura/{id}` - Fatura sil (stok ve bakiye geri alınır)

## 🎨 Frontend (Angular)

### Dosya Yapısı

#### Models
- `src/app/models/stok.model.ts`
- `src/app/models/cari-hesap.model.ts`
- `src/app/models/fatura.model.ts`

#### Services
- `src/app/services/stok.service.ts`
- `src/app/services/cari-hesap.service.ts`
- `src/app/services/fatura.service.ts`

#### Components
- `src/app/components/stok/`
  - stok.component.ts/html/css
- `src/app/components/cari-hesap/`
  - cari-hesap.component.ts/html/css
- `src/app/components/fatura/`
  - fatura.component.ts/html/css

### Routes
- `/stok` - Stok yönetimi sayfası
- `/cari-hesap` - Cari hesap yönetimi sayfası
- `/fatura` - Fatura yönetimi sayfası

## 🚀 Özellikler

### Stok Modülü
- ✅ Stok ekleme/düzenleme/silme
- ✅ Stok kodu benzersizlik kontrolü
- ✅ Birim seçimi (Adet, Kg, Lt, Mt, M2, M3)
- ✅ Minimum stok uyarısı (sarı renk)
- ✅ Aktif/Pasif stok durumu

### Cari Hesap Modülü
- ✅ Cari hesap ekleme/düzenleme/silme
- ✅ Vergi numarası ve daire bilgileri
- ✅ İletişim bilgileri (Adres, Tel, Email)
- ✅ Bakiye takibi (Borç/Alacak gösterimi)
- ✅ Firma ilişkilendirme

### Fatura Modülü
- ✅ Otomatik fatura no üretimi (Format: F{YılAy}{Sıra})
- ✅ Çoklu kalem ekleme
- ✅ Stok seçimi ve otomatik fiyat dolumu
- ✅ KDV hesaplama (%0, %1, %10, %20)
- ✅ Otomatik toplam hesaplama
- ✅ Stok düşme (fatura kesildiğinde)
- ✅ Cari hesap bakiye güncelleme
- ✅ Önceki ve sonraki bakiye gösterimi
- ✅ Fatura detay görüntüleme
- ✅ Fatura silme (stok ve bakiye geri alma)

## 🔄 İş Akışı

### Fatura Kesme Süreci
1. Yeni fatura formunda otomatik fatura no oluşturulur
2. Cari hesap seçilir
3. Stoklar kalem kalem eklenir:
   - Stok seçilince otomatik fiyat gelir
   - Miktar ve KDV oranı girilir
   - Hesaplamalar otomatik yapılır
4. Fatura kesildiğinde:
   - Her kalem için stok adedi düşer
   - Cari hesap bakiyesi güncellenir
   - Önceki ve sonraki bakiye kaydedilir

### Fatura Silme Süreci
1. Fatura silindiğinde:
   - Stok adedi geri eklenir
   - Cari hesap bakiyesi geri alınır
   - Soft delete yapılır (IsDeleted = true)

## 📊 Veri Doğrulama

### Backend Validations
- Stok kodu benzersizliği
- Cari kodu benzersizliği
- Fatura no benzersizliği
- Stok yeterlilik kontrolü
- Minimum değer kontrolleri

### Frontend Validations
- Required field kontrolleri
- Number validations
- Email format kontrolü
- Form validations

## 🎯 Kullanım

### Başlangıç Adımları

1. **Backend Başlatma:**
```bash
cd c:\ProccesModuleBackend
dotnet run --project ProcessModule.WebAPI
```

2. **Frontend Başlatma:**
```bash
cd c:\ProccesModuleUi
npm install  # İlk seferde
npm start
```

3. **Erişim:**
- Backend API: `https://localhost:7xxx` (SSL) veya `http://localhost:5xxx`
- Frontend UI: `http://localhost:4200`

### Örnek Kullanım Senaryosu

1. **Stok Ekleme:**
   - `/stok` sayfasına git
   - "Yeni Stok Ekle" butonuna tıkla
   - Bilgileri doldur ve kaydet

2. **Cari Hesap Ekleme:**
   - `/cari-hesap` sayfasına git
   - "Yeni Cari Ekle" butonuna tıkla
   - Müşteri bilgilerini doldur ve kaydet

3. **Fatura Kesme:**
   - `/fatura` sayfasına git
   - "Yeni Fatura Kes" butonuna tıkla
   - Cari hesap seç
   - Stokları ekle
   - Faturayı kes

## 🔮 Gelecek Geliştirmeler

### PDF Oluşturma (Planlanan)
Backend'e PDF generation servisi eklenecek:
- QuestPDF veya DinkToPDF kütüphanesi kullanılacak
- Fatura PDF endpoint eklenecek: `GET /api/fatura/{id}/pdf`
- Frontend'den PDF indirme özelliği aktif hale gelecek

### Raporlama (Planlanan)
- Stok listesi raporu
- Cari ekstre raporu
- Fatura listesi raporu
- Dönemsel satış raporu

### Excel Export (Planlanan)
- Tüm listelerde Excel export özelliği
- EPPlus kütüphanesi kullanılacak

## 📝 Notlar

- Tüm tarih alanları UTC olarak saklanır
- Soft delete pattern kullanılır (IsDeleted flag)
- BaseEntity pattern ile ortak alanlar: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
- JWT Authentication ile API korunur
- CORS yapılandırması Angular için açık

## 🐛 Bilinen Sorunlar

- Sidebar menü sistemi dynamic API'den geldiği için manuel olarak `/stok`, `/cari-hesap`, `/fatura` URL'lerini yazmak gerekiyor
- PDF generation henüz implement edilmedi

## 📞 Destek

Herhangi bir sorun veya soru için:
- Backend endpoint testleri için Swagger UI kullanın
- Frontend hataları için browser console'u kontrol edin
- Database migration sorunları için EF Core migration komutlarını kontrol edin

---

**Geliştirme Tarihi:** 01 Nisan 2026
**Geliştirici:** GitHub Copilot
**Versiyon:** 1.0.0
