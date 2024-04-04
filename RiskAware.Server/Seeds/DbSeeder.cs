using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class DbSeeder
    {
        public static async Task SeedAll(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Synchronous seeding
            VehicleSeed.Seed(context);

            // Asynchronous seeding
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await UserSeed.Seed(userManager);
        }
    }
}
