using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PartnerManager.Infrastructure.Data
{
    // Enables dotnet-ef tools to create the DbContext at design time for migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Try environment variable first
            var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(conn))
            {
                // fallback to appsettings.json if present
                var basePath = Directory.GetCurrentDirectory();
                var builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddEnvironmentVariables();
                var config = builder.Build();
                conn = config.GetConnectionString("DefaultConnection") ?? "Host=localhost;Port=5432;Database=partnerdb;Username=postgres;Password=changeme";
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(conn);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
