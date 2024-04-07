using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class UserSeed
    {
        public static async Task Seed(UserManager<User> userManager, AppDbContext context)
        {
            const string adminId = "d6f46418-2c21-43f8-b167-162fb5e3a999";
            if (await userManager.FindByIdAsync(adminId) == null)
            {
                const string adminEmail = "admin@google.com";
                var systemRoleId = Guid.Parse("e4499f9c-a59c-4b25-a86e-81ca032dc313");
                var systemRole = await context.SystemRoles.FindAsync(systemRoleId);
                var adminUser = new User()
                {
                    Id = adminId,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "HonzaAdmin",
                    LastName = "Zvesnice",
                    SystemRole = systemRole
                };

                await userManager.CreateAsync(adminUser, "Admin123");
            }

            var systemRoleIdBase = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6");
            var systemRoleBase = await context.SystemRoles.FindAsync(systemRoleIdBase);
            const string baseId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8";
            if (await userManager.FindByIdAsync(baseId) == null)
            {
                const string baseEmail = "pb@google.com";
                var adminUser = new User()
                {
                    Id = baseId,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Pepa",
                    LastName = "Brnak",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseId2 = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2";
            if (await userManager.FindByIdAsync(baseId2) == null)
            {
                const string baseEmail = "zb@google.com";
                var adminUser = new User()
                {
                    Id = baseId2,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Zdenda",
                    LastName = "Branik",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseId3 = "84c8b270-14e5-4158-bcde-a76c6edc4cf7";
            if (await userManager.FindByIdAsync(baseId3) == null)
            {
                const string baseEmail = "ds@google.com";
                var adminUser = new User()
                {
                    Id = baseId3,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "David",
                    LastName = "Sicko",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseId4 = "5862be25-6467-450e-81fa-1cac9578650b";
            if (await userManager.FindByIdAsync(baseId4) == null)
            {
                const string baseEmail = "jk@google.com";
                var adminUser = new User()
                {
                    Id = baseId4,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Jirka",
                    LastName = "Kohout",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }
        }
    }
}
