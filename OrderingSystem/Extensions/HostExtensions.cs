using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Core.Entities.Identity;
using OrderingSystem.Repository.Data;
using OrderingSystem.Repository.Identity.DataSeed;

namespace OrderingSystem.API.Extensions
{
    public static class HostExtensions
    {
        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var _dbContext = services.GetRequiredService<AppDbContext>();
                var _userManager = services.GetRequiredService<UserManager<Customer>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await _dbContext.Database.MigrateAsync();

                await AppDbContextSeed.SeedUserAsync(_userManager, roleManager);

                await AppDbContextSeed.SeedAsync(_dbContext, loggerFactory);
       
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("Database Migration");
                logger.LogError(ex, "An error occurred while applying the database migration.");
            }
        }
    }
}
