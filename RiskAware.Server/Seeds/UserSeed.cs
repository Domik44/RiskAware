using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class UserSeed
    {
        public static async Task Seed(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            const string adminId = "d6f46418-2c21-43f8-b167-162fb5e3a999";
            if (await userManager.FindByIdAsync(adminId) == null)
            {
                const string adminEmail = "admin@google.com";
                var systemRoleId = 1;
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

                string roleAdmin = "Admin";
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));

                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin is not null)
                {
                    await userManager.AddToRoleAsync(admin, roleAdmin);
                }
            }

            var systemRoleIdBase = 2;
            var systemRoleBase = await context.SystemRoles.FindAsync(systemRoleIdBase);
            const string baseId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8";
            if (await userManager.FindByIdAsync(baseId) == null)
            {
                const string baseEmail = "pb@google.com";
                var baseUser = new User()
                {
                    Id = baseId,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Pepa",
                    LastName = "Brnak",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }

            const string baseId2 = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2";
            if (await userManager.FindByIdAsync(baseId2) == null)
            {
                const string baseEmail = "zb@google.com";
                var baseUser = new User()
                {
                    Id = baseId2,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Zdenda",
                    LastName = "Branik",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }

            const string baseId3 = "84c8b270-14e5-4158-bcde-a76c6edc4cf7";
            if (await userManager.FindByIdAsync(baseId3) == null)
            {
                const string baseEmail = "ds@google.com";
                var baseUser = new User()
                {
                    Id = baseId3,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "David",
                    LastName = "Sicko",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }

            const string baseId4 = "5862be25-6467-450e-81fa-1cac9578650b";
            if (await userManager.FindByIdAsync(baseId4) == null)
            {
                const string baseEmail = "jk@google.com";
                var baseUser = new User()
                {
                    Id = baseId4,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Jirka",
                    LastName = "Kohout",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }

            const string baseId5 = "31ab7787-60dc-4309-a069-4c30fc837ef0";
            if (await userManager.FindByIdAsync(baseId5) == null)
            {
                const string baseEmail = "jp@google.com";
                var baseUser = new User()
                {
                    Id = baseId5,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Jana",
                    LastName = "Pálková",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }

            const string baseId6 = "12749a7a-100b-4e69-a234-7d059e508d5b";
            if (await userManager.FindByIdAsync(baseId6) == null)
            {
                const string baseEmail = "bm@google.com";
                var baseUser = new User()
                {
                    Id = baseId6,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Béďa",
                    LastName = "Medvídek",
                    SystemRole = systemRoleBase
                };

                await userManager.CreateAsync(baseUser, "Basic123");
            }
        }
    }
}
