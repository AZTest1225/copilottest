$ErrorActionPreference = "Continue"
$env:ASPNETCORE_ENVIRONMENT = 'Development'
Set-Location "d:\Codes\CopilotTest\backend\src\PartnerManager.Api"
Write-Host "Starting API on http://localhost:54413..." -ForegroundColor Green
dotnet run
