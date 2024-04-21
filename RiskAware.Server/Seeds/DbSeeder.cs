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
            SystemRoleSeed.Seed(context);

            // Asynchronous seeding
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await UserSeed.Seed(userManager, roleManager, context);

            RiskProjectSeed.Seed(context);
            ProjectPhaseSeed.Seed(context);
            ProjectRoleSeed.Seed(context);
            RiskCategorySeed.Seed(context);
            RiskSeed.Seed(context);
            RiskHistorySeed.Seed(context);
            CommentSeed.Seed(context);
        }
    }
}
