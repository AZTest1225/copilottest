using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using PartnerManager.Infrastructure.Config;
using Microsoft.Extensions.Configuration;

namespace PartnerManager.Infrastructure.Data
{
    // Enables dotnet-ef tools to create the DbContext at design time for migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var cfg = BuildConfig();
            var conn = cfg.DefaultConnection ?? "Host=localhost;Port=5432;Database=partnerdb;Username=postgres;Password=demo12!@";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(conn);
            return new AppDbContext(optionsBuilder.Options);
        }

        private static CConfig BuildConfig()
        {
            // Prefer CONNECTION_STRING env override
            var connEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            var basePath = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();
            var cfg = new CConfig();
            configuration.Bind(cfg);
            if (!string.IsNullOrWhiteSpace(connEnv))
            {
                cfg.DefaultConnection = connEnv;
            }
            return cfg;
        }
    }
}
