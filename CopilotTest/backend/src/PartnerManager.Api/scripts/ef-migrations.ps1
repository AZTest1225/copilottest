# PowerShell script to create initial EF Core migration and apply it
# Usage: Open PowerShell in this folder and run: .\ef-migrations.ps1

param(
    [string]$MigrationName = "InitialCreate"
)

Write-Host "Running EF Core migration script in project: $(Get-Location)"

# Ensure dotnet-ef is available
try {
    dotnet ef --version | Out-Null
} catch {
    Write-Host "dotnet-ef not found. Install it with:`n  dotnet tool install --global dotnet-ef --version 7.0.0"
    exit 1
}

Write-Host "Restoring and building project..."
dotnet restore

Write-Host "Adding migration: $MigrationName"
dotnet ef migrations add $MigrationName -o Migrations

Write-Host "Applying database update"
dotnet ef database update

Write-Host "Migration and update complete."
