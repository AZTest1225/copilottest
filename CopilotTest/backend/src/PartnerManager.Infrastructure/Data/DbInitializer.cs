using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }

                if (!await context.Users.AnyAsync())
                {
                    var admin = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        Role = Role.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Users.Add(admin);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Seeded admin user: admin@example.com / Admin@123");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
