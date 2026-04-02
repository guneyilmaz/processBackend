# Portable ProcessModule Package Creator
# USB'ye kopyalanabilir, her PC'de çalışır

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Portable Package Oluşturuluyor" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$PortablePath = "C:\ProcessModule-Portable"

Write-Host "1. Backend portable publish..." -ForegroundColor Yellow
cd C:\ProccesModuleBackend\ProcessModule.WebAPI
dotnet publish -c Release -o "$PortablePath\App\Backend" `
    --self-contained true `
    --runtime win-x64 `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true

Write-Host ""
Write-Host "2. Frontend build..." -ForegroundColor Yellow
cd C:\ProccesModuleUi
npm install
npm run build
Copy-Item -Path "dist\procces-module-ui\browser\*" -Destination "$PortablePath\App\Frontend" -Recurse -Force

Write-Host ""
Write-Host "3. SQL Server LocalDB portable files..." -ForegroundColor Yellow
# LocalDB portable installer'ı indir
$localDbUrl = "https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi"
Invoke-WebRequest -Uri $localDbUrl -OutFile "$PortablePath\SqlLocalDB.msi"

Write-Host ""
Write-Host "4. Başlatma scripti..." -ForegroundColor Yellow

$startScript = @'
@echo off
title ProcessModule Portable
cd /d "%~dp0"

echo ========================================
echo ProcessModule Portable Edition
echo ========================================
echo.

REM LocalDB'yi kontrol et
if not exist "%~dp0Data\LocalDB" (
    echo LocalDB ilk kurulum yapiliyor...
    msiexec /i SqlLocalDB.msi /qn IACCEPTSQLLOCALDBLICENSETERMS=YES
    mkdir "%~dp0Data\LocalDB"
)

REM Backend'i başlat
echo Backend baslatiliyor...
start "ProcessModule API" /D "%~dp0App\Backend" ProcessModule.WebAPI.exe

timeout /t 5 /nobreak > nul

REM Frontend'i başlat  
echo Frontend baslatiliyor...
start "ProcessModule UI" /D "%~dp0App\Frontend" http-server . -p 4200 -c-1

echo.
echo ✅ Uygulama Baslatildi!
echo.
echo Tarayici: http://localhost:4200
echo API:      http://localhost:5000
echo.
echo Bu pencereyi kapatmayin!
pause
'@

$startScript | Out-File -FilePath "$PortablePath\START-PORTABLE.bat" -Encoding ASCII

# README oluştur
$readme = @"
# ProcessModule Portable Edition

## Kurulum Gereksinimleri:
HIÇBIR ŞEY! Sadece Windows 10/11

## Kullanım:
1. START-PORTABLE.bat dosyasını çalıştır
2. İlk çalıştırmada SQL LocalDB kurulumu yapılır (1 dakika)
3. Tarayıcıda http://localhost:4200 adresine git

## Dosya Yapısı:
- App\Backend\      → Backend API
- App\Frontend\     → Frontend UI
- Data\             → Veritabanı dosyaları
- SqlLocalDB.msi    → LocalDB installer

## Not:
- Bu paket USB'ye kopyalanabilir
- Her Windows bilgisayarda çalışır
- İnternet bağlantısı gerekmez (kurulumdan sonra)
- Data klasörünü yedekleyin (veritabanı burada)
"@

$readme | Out-File -FilePath "$PortablePath\README.txt" -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Portable Package Hazır!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Klasör: $PortablePath" -ForegroundColor Green
Write-Host "Boyut: ~" -NoNewline
$size = (Get-ChildItem -Path $PortablePath -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB
Write-Host "$([math]::Round($size, 2)) MB" -ForegroundColor Cyan
Write-Host ""
Write-Host "Bu klasörü tamamını:" -ForegroundColor Yellow
Write-Host "- USB'ye kopyalayın" -ForegroundColor White
Write-Host "- Ağ paylaşımına atın" -ForegroundColor White
Write-Host "- Başka bilgisayara taşıyın" -ForegroundColor White
Write-Host ""
Write-Host "START-PORTABLE.bat ile başlatın!" -ForegroundColor Cyan

explorer $PortablePath
