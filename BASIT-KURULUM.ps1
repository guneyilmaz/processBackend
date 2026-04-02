# ProcessModule - Basit Kurulum (IIS'siz)
# Tek komutla çalıştır!

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "ProcessModule - Basit Kurulum" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$InstallPath = "C:\ProcessModule"

# 1. BACKEND PUBLISH
Write-Host "1. Backend hazırlanıyor..." -ForegroundColor Yellow
cd C:\ProccesModuleBackend\ProcessModule.WebAPI
dotnet publish -c Release -o "$InstallPath\Backend" --self-contained false

# 2. FRONTEND BUILD
Write-Host ""
Write-Host "2. Frontend build alınıyor..." -ForegroundColor Yellow
cd C:\ProccesModuleUi

# Environment dosyasını otomatik güncelle
$envFile = "src\environments\environment.prod.ts"
$envContent = @"
export const environment = {
  production: true,
  apiUrl: 'http://localhost:5000/api'
};
"@
$envContent | Out-File -FilePath $envFile -Encoding UTF8

npm install
npm run build

# Build dosyalarını kopyala
Copy-Item -Path "dist\procces-module-ui\browser\*" -Destination "$InstallPath\Frontend" -Recurse -Force

# 3. SQL EXPRESS KONTROL
Write-Host ""
Write-Host "3. SQL Server kontrol ediliyor..." -ForegroundColor Yellow
$sqlService = Get-Service -Name "MSSQL$SQLEXPRESS" -ErrorAction SilentlyContinue
if ($sqlService) {
    Write-Host "✅ SQL Server Express bulundu" -ForegroundColor Green
} else {
    Write-Host "⚠️  SQL Server bulunamadı!" -ForegroundColor Red
    Write-Host "SQL Server Express indir: https://go.microsoft.com/fwlink/?linkid=866662" -ForegroundColor Yellow
}

# 4. BAŞLATMA SCRIPT'LERİ OLUŞTUR
Write-Host ""
Write-Host "4. Başlatma scriptleri oluşturuluyor..." -ForegroundColor Yellow

# Backend başlatma scripti
$backendStart = @'
@echo off
title ProcessModule Backend API
cd /d "%~dp0Backend"

echo ========================================
echo ProcessModule Backend API
echo ========================================
echo.
echo API Running on: http://localhost:5000
echo Press Ctrl+C to stop
echo.

dotnet ProcessModule.WebAPI.dll --urls "http://localhost:5000"
pause
'@
$backendStart | Out-File -FilePath "$InstallPath\Start-Backend.bat" -Encoding ASCII

# Frontend başlatma scripti (http-server ile)
$frontendStart = @'
@echo off
title ProcessModule Frontend
cd /d "%~dp0Frontend"

echo ========================================
echo ProcessModule Frontend UI
echo ========================================
echo.
echo UI Running on: http://localhost:4200
echo Press Ctrl+C to stop
echo.

npx http-server . -p 4200 -c-1 --proxy http://localhost:5000?
pause
'@
$frontendStart | Out-File -FilePath "$InstallPath\Start-Frontend.bat" -Encoding ASCII

# Hepsini başlat scripti
$startAll = @'
@echo off
title ProcessModule Starter
echo ========================================
echo ProcessModule - Basit Başlatıcı
echo ========================================
echo.
echo Backend ve Frontend başlatılıyor...
echo.

start "Backend API" cmd /k "%~dp0Start-Backend.bat"
timeout /t 5 /nobreak > nul

start "Frontend UI" cmd /k "%~dp0Start-Frontend.bat"

echo.
echo ✅ Uygulama başlatıldı!
echo.
echo Frontend: http://localhost:4200
echo Backend:  http://localhost:5000
echo.
echo Windows'u kapatmayın, programlar çalışıyor!
echo.
pause
'@
$startAll | Out-File -FilePath "$InstallPath\START.bat" -Encoding ASCII

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Kurulum Tamamlandı!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Klasör: $InstallPath" -ForegroundColor Green
Write-Host ""
Write-Host "🚀 BAŞLATMAK İÇİN:" -ForegroundColor Yellow
Write-Host "   $InstallPath\START.bat dosyasını çift tıkla!" -ForegroundColor Cyan
Write-Host ""
Write-Host "VEYA ayrı ayrı:" -ForegroundColor Yellow
Write-Host "   1. Start-Backend.bat (önce backend'i başlat)" -ForegroundColor White
Write-Host "   2. Start-Frontend.bat (sonra frontend'i başlat)" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  ÖNEMLİ:" -ForegroundColor Red
Write-Host "- SQL Server Express kurulu olmalı" -ForegroundColor White
Write-Host "- Database migration çalıştırılmış olmalı" -ForegroundColor White
Write-Host "- Node.js kurulu olmalı (frontend için)" -ForegroundColor White
Write-Host ""

# Explorer'da klasörü aç
explorer $InstallPath

Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
