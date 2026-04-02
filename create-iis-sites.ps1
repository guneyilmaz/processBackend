# IIS Site Oluşturma Script'i - YÖNETİCİ OLARAK ÇALIŞTIRIN!
# ProcessModule için IIS site ve app pool oluşturur

#Requires -RunAsAdministrator

Import-Module WebAdministration

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "ProcessModule IIS Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Parametreler
$BackendPath = "C:\Publish\ProcessModuleAPI"
$FrontendPath = "C:\Publish\ProcessModuleUI"
$BackendPort = 5000
$FrontendPort = 80

# IIS modüllerini kontrol et
Write-Host "Checking IIS installation..." -ForegroundColor Yellow
if (!(Get-Module -ListAvailable -Name WebAdministration)) {
    Write-Host "❌ IIS is not installed or WebAdministration module not found!" -ForegroundColor Red
    Write-Host "Please install IIS first from Windows Features." -ForegroundColor Yellow
    exit 1
}
Write-Host "✅ IIS found" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "BACKEND API SETUP" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Backend App Pool
$BackendAppPoolName = "ProcessModuleAPI"
Write-Host "Creating application pool: $BackendAppPoolName..." -ForegroundColor Yellow

if (Test-Path "IIS:\AppPools\$BackendAppPoolName") {
    Write-Host "⚠️  App pool already exists, removing..." -ForegroundColor Yellow
    Remove-WebAppPool -Name $BackendAppPoolName
}

New-WebAppPool -Name $BackendAppPoolName
Set-ItemProperty "IIS:\AppPools\$BackendAppPoolName" -Name "managedRuntimeVersion" -Value ""
Set-ItemProperty "IIS:\AppPools\$BackendAppPoolName" -Name "enable32BitAppOnWin64" -Value $false
Write-Host "✅ App pool created" -ForegroundColor Green

# Backend Website
$BackendSiteName = "ProcessModule.API"
Write-Host "Creating website: $BackendSiteName..." -ForegroundColor Yellow

if (Test-Path "IIS:\Sites\$BackendSiteName") {
    Write-Host "⚠️  Site already exists, removing..." -ForegroundColor Yellow
    Remove-Website -Name $BackendSiteName
}

# Klasör kontrolü
if (!(Test-Path $BackendPath)) {
    Write-Host "⚠️  Backend path not found: $BackendPath" -ForegroundColor Yellow
    Write-Host "    Please publish backend first using publish-backend.ps1" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $BackendPath -Force | Out-Null
}

New-Website -Name $BackendSiteName `
    -PhysicalPath $BackendPath `
    -ApplicationPool $BackendAppPoolName `
    -Port $BackendPort `
    -Force

Write-Host "✅ Backend site created on port $BackendPort" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FRONTEND UI SETUP" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Frontend App Pool
$FrontendAppPoolName = "ProcessModuleUI"
Write-Host "Creating application pool: $FrontendAppPoolName..." -ForegroundColor Yellow

if (Test-Path "IIS:\AppPools\$FrontendAppPoolName") {
    Write-Host "⚠️  App pool already exists, removing..." -ForegroundColor Yellow
    Remove-WebAppPool -Name $FrontendAppPoolName
}

New-WebAppPool -Name $FrontendAppPoolName
Set-ItemProperty "IIS:\AppPools\$FrontendAppPoolName" -Name "managedRuntimeVersion" -Value ""
Write-Host "✅ App pool created" -ForegroundColor Green

# Frontend Website
$FrontendSiteName = "ProcessModule.UI"
Write-Host "Creating website: $FrontendSiteName..." -ForegroundColor Yellow

if (Test-Path "IIS:\Sites\$FrontendSiteName") {
    Write-Host "⚠️  Site already exists, removing..." -ForegroundColor Yellow
    Remove-Website -Name $FrontendSiteName
}

# Klasör kontrolü
if (!(Test-Path $FrontendPath)) {
    Write-Host "⚠️  Frontend path not found: $FrontendPath" -ForegroundColor Yellow
    Write-Host "    Please build frontend first using publish-frontend.ps1" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $FrontendPath -Force | Out-Null
}

# Port 80 kullanımda mı kontrol et
$Port80InUse = Get-Website | Where-Object { $_.bindings.Collection.bindingInformation -like "*:80:*" }
if ($Port80InUse -and $FrontendPort -eq 80) {
    Write-Host "⚠️  Port 80 is already in use by: $($Port80InUse.name)" -ForegroundColor Yellow
    Write-Host "    Using port 8080 instead..." -ForegroundColor Yellow
    $FrontendPort = 8080
}

New-Website -Name $FrontendSiteName `
    -PhysicalPath $FrontendPath `
    -ApplicationPool $FrontendAppPoolName `
    -Port $FrontendPort `
    -Force

Write-Host "✅ Frontend site created on port $FrontendPort" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "STARTING SITES" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

Start-Website -Name $BackendSiteName
Start-Website -Name $FrontendSiteName

Write-Host "✅ Sites started" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ IIS Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Backend API URL: " -NoNewline
Write-Host "http://localhost:$BackendPort" -ForegroundColor Green
Write-Host "Frontend UI URL: " -NoNewline
Write-Host "http://localhost:$FrontendPort" -ForegroundColor Green
Write-Host ""
Write-Host "⚠️  IMPORTANT REMINDERS:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Make sure .NET 9.0 Hosting Bundle is installed" -ForegroundColor White
Write-Host "   Download: https://dotnet.microsoft.com/download/dotnet/9.0" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Configure SQL Server connection string in:" -ForegroundColor White
Write-Host "   $BackendPath\appsettings.json" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Run database migrations" -ForegroundColor White
Write-Host ""
Write-Host "4. Install IIS URL Rewrite Module for frontend routing:" -ForegroundColor White
Write-Host "   https://www.iis.net/downloads/microsoft/url-rewrite" -ForegroundColor Cyan
Write-Host ""
Write-Host "5. Update frontend API URL in:" -ForegroundColor White
Write-Host "   src\environments\environment.prod.ts" -ForegroundColor Cyan
Write-Host "   Set apiUrl to: 'http://localhost:$BackendPort/api'" -ForegroundColor Cyan
Write-Host ""
Write-Host "6. Configure Windows Firewall if accessing from other machines" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
