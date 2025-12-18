# PartnerManager.Infrastructure

This project contains EF Core models, `AppDbContext`, and repositories. It's set up for Code-First migrations.

Generating migrations (recommended from solution root):

1. Ensure `dotnet-ef` tool is installed:

```powershell
dotnet tool install --global dotnet-ef --version 7.0.0
```

2. From the solution or API project folder, run dotnet ef specifying the target project (Infrastructure) and startup project (Api):

```powershell
# from backend/src/PartnerManager.Api
# create migration in Infrastructure project
dotnet ef migrations add InitialCreate -p ..\PartnerManager.Infrastructure\PartnerManager.Infrastructure.csproj -s PartnerManager.Api -o ..\PartnerManager.Infrastructure\Migrations

# apply migration
dotnet ef database update -p ..\PartnerManager.Infrastructure\PartnerManager.Infrastructure.csproj -s PartnerManager.Api
```

Notes:
- The `DesignTimeDbContextFactory` will pick up connection string from env var `CONNECTION_STRING` or `appsettings.json` if present.
- Ensure the API project references the Infrastructure project (already configured) so the startup code can resolve `AppDbContext`.

If you want, I can generate a placeholder migration file, but it's preferable to run the commands above locally so the migration reflects your environment.
