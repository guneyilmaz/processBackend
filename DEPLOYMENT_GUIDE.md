# ProcessModule Deployment Rehberi

## 🎯 Gereksinimler

### Sunucu Gereksinimleri:
- Windows Server 2016 veya üzeri / Windows 10/11
- IIS 10.0 veya üzeri
- SQL Server 2019 veya üzeri (Express Edition yeterli)
- .NET 9.0 Hosting Bundle
- Minimum 4GB RAM, 10GB Disk

### Yazılımlar:
1. **SQL Server 2019+**
   - İndir: https://www.microsoft.com/sql-server/sql-server-downloads
   - Express Edition ücretsiz
   
2. **.NET 9.0 Hosting Bundle**
   - İndir: https://dotnet.microsoft.com/download/dotnet/9.0
   - "ASP.NET Core Runtime & Hosting Bundle" seçin
   
3. **IIS** (Windows Features'dan etkinleştirin)

---

## 📦 1. BACKEND DEPLOYMENT

### Adım 1.1: Backend'i Publish Et

```powershell
cd C:\ProccesModuleBackend\ProcessModule.WebAPI
dotnet publish -c Release -o C:\Publish\ProcessModuleAPI
```

### Adım 1.2: SQL Server Kurulumu

1. SQL Server Management Studio (SSMS) ile bağlan
2. Yeni database oluştur:
```sql
CREATE DATABASE ProccesModule;
GO
```

3. SQL Authentication için kullanıcı oluştur:
```sql
USE [master]
GO
CREATE LOGIN [ProcessModuleUser] WITH PASSWORD=N'YourStrongPassword123!', 
    DEFAULT_DATABASE=[ProccesModule], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [ProccesModule]
GO
CREATE USER [ProcessModuleUser] FOR LOGIN [ProcessModuleUser]
GO
ALTER ROLE [db_owner] ADD MEMBER [ProcessModuleUser]
GO
```

### Adım 1.3: Connection String Ayarla

`C:\Publish\ProcessModuleAPI\appsettings.json` dosyasını düzenle:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProccesModule;User Id=ProcessModuleUser;Password=YourStrongPassword123!;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "YourProductionSecretKeyThatIsAtLeast32Characters!",
    "Issuer": "ProcessModuleAPI",
    "Audience": "ProcessModuleClient",
    "ExpirationMinutes": 120
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Adım 1.4: Migration Çalıştır

```powershell
# Development bilgisayarında (migration dosyaları varsa):
cd C:\ProccesModuleBackend
dotnet ef database update -p ProcessModule.Persistence -s ProcessModule.WebAPI --connection "Server=SUNUCU_IP;Database=ProccesModule;User Id=ProcessModuleUser;Password=YourStrongPassword123!;TrustServerCertificate=True"

# VEYA sunucuda:
cd C:\Publish\ProcessModuleAPI
dotnet ProcessModule.WebAPI.dll --migrate
```

**⚠️ Önemli:** Migration sonrası admin kullanıcısı ve menüleri ekleyin (SQL scriptler).

### Adım 1.5: IIS'te Backend Site Oluştur

1. **IIS Manager** aç (Win+R → inetmgr)
2. **Application Pools** → **Add Application Pool**
   - Name: `ProcessModuleAPI`
   - .NET CLR Version: **No Managed Code**
   - Managed Pipeline Mode: Integrated
3. **Sites** → **Add Website**
   - Site name: `ProcessModule.API`
   - Application pool: `ProcessModuleAPI`
   - Physical path: `C:\Publish\ProcessModuleAPI`
   - Binding: 
     - Type: http
     - IP: All Unassigned
     - Port: `5000`
     - Hostname: (boş bırak veya `api.yourcompany.com`)
4. **Application Pool → Advanced Settings**
   - Identity: `ApplicationPoolIdentity` veya SQL erişimi olan bir hesap

### Adım 1.6: API Testi

Tarayıcıda: `http://localhost:5000/api/health` veya `http://SERVER_IP:5000/api/health`

---

## 🎨 2. FRONTEND DEPLOYMENT

### Adım 2.1: Environment Ayarları

`C:\ProccesModuleUi\src\environments\environment.prod.ts` düzenle:

```typescript
export const environment = {
  production: true,
  apiUrl: 'http://YOUR_SERVER_IP:5000/api'  // veya domain
};
```

### Adım 2.2: Angular Build

```powershell
cd C:\ProccesModuleUi
npm install
npm run build
# veya production build:
ng build --configuration production
```

Build sonucu: `C:\ProccesModuleUi\dist\procces-module-ui\browser`

### Adım 2.3: IIS'te Frontend Site Oluştur

1. **Application Pools** → **Add Application Pool**
   - Name: `ProcessModuleUI`
   - .NET CLR Version: **No Managed Code**
2. **Sites** → **Add Website**
   - Site name: `ProcessModule.UI`
   - Application pool: `ProcessModuleUI`
   - Physical path: `C:\Publish\ProcessModuleUI`
   - Binding:
     - Type: http
     - Port: `80` (veya 8080)
3. Build dosyalarını kopyala:
```powershell
xcopy "C:\ProccesModuleUi\dist\procces-module-ui\browser\*" "C:\Publish\ProcessModuleUI\" /E /I /Y
```

### Adım 2.4: web.config Ekle (Angular için)

`C:\Publish\ProcessModuleUI\web.config` oluştur:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
    <staticContent>
      <remove fileExtension=".json" />
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
      <mimeMap fileExtension=".woff2" mimeType="application/font-woff2" />
    </staticContent>
    <httpProtocol>
      <customHeaders>
        <add name="Access-Control-Allow-Origin" value="*" />
        <add name="Access-Control-Allow-Methods" value="GET,POST,PUT,DELETE,OPTIONS" />
        <add name="Access-Control-Allow-Headers" value="Content-Type,Authorization" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>
```

### Adım 2.5: URL Rewrite Modülü

IIS'te Angular routing çalışması için:
- İndir: [IIS URL Rewrite Module](https://www.iis.net/downloads/microsoft/url-rewrite)
- Kurulum yap ve IIS'i restart et

---

## 🔒 3. GÜVENLİK AYARLARI

### Backend:
- ✅ `appsettings.json` dosya izinlerini kısıtla (sadece IIS AppPool kullanıcısı okuyabilsin)
- ✅ JWT SecretKey'i değiştir
- ✅ HTTPS ayarla (Let's Encrypt veya self-signed certificate)
- ✅ CORS ayarlarını production domain ile sınırla

### SQL Server:
- ✅ SQL Authentication kullan
- ✅ Güçlü şifre kullan
- ✅ Firewall kuralları (sadece local erişim)
- ✅ Backup planı oluştur

### IIS:
- ✅ Windows Firewall'da port 80, 5000'i aç
- ✅ Antivirus exceptions ekle (`C:\Publish` klasörü)

---

## 🧪 4. TEST

1. **Backend Test:**
   ```powershell
   curl http://localhost:5000/api/health
   ```

2. **Frontend Test:**
   - Tarayıcı: `http://localhost` veya `http://SERVER_IP`
   - Login sayfası açılmalı

3. **Database Test:**
   - SSMS ile bağlan
   - Tabloları kontrol et (Users, Companies, Stok, CariHesap, Fatura...)

---

## 🔧 SORUN GİDERME

### Backend 500 Hatası:
```powershell
# IIS logs:
C:\inetpub\logs\LogFiles\W3SVC1\

# Application Event Viewer:
eventvwr.msc → Windows Logs → Applications
```

### Frontend API Bağlanamıyor:
- `environment.prod.ts` API URL kontrol et
- CORS ayarları kontrol et
- Network tab'de request URL'yi kontrol et

### Database Bağlantı Hatası:
- SQL Server servisi çalışıyor mu?
- Connection string doğru mu?
- Firewall SQL port'u (1433) açık mı?
- SQL Browser servisi çalışıyor mu?

---

## 📊 PERFORMANS

- Application Pool → Recycling ayarları
- Static file compression (IIS)
- SQL Server memory limitleri
- Connection pooling ayarları

---

## 🔄 GÜNCELLEME

### Backend Güncelleme:
```powershell
# 1. IIS'te site'ı durdur
# 2. Yeni publish yap
cd C:\ProccesModuleBackend\ProcessModule.WebAPI
dotnet publish -c Release -o C:\Publish\ProcessModuleAPI
# 3. IIS'te site'ı başlat
```

### Frontend Güncelleme:
```powershell
cd C:\ProccesModuleUi
ng build --configuration production
xcopy "dist\procces-module-ui\browser\*" "C:\Publish\ProcessModuleUI\" /E /I /Y
```

---

## 📞 YEDEKLEME

### Database Backup:
```sql
BACKUP DATABASE [ProccesModule] 
TO DISK = N'C:\Backups\ProccesModule_FULL.bak' 
WITH FORMAT, INIT, COMPRESSION, STATS = 10
GO
```

### Otomatik Backup:
SQL Server Agent → Maintenance Plans ile günlük yedek oluşturun.

---

## ✅ KONTROL LİSTESİ

- [ ] .NET 9.0 Hosting Bundle kuruldu
- [ ] SQL Server kuruldu ve ProccesModule DB oluşturuldu
- [ ] Migration çalıştırıldı
- [ ] Admin kullanıcısı ve menüler eklendi
- [ ] Backend IIS'te publish edildi ve çalışıyor
- [ ] Frontend build alındı ve IIS'te host edildi
- [ ] URL Rewrite modülü kuruldu
- [ ] Firewall portları açıldı
- [ ] Login testi yapıldı ✅
- [ ] CRUD işlemleri test edildi
- [ ] HTTPS yapılandırıldı (opsiyonel)
- [ ] Backup planı oluşturuldu
