using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class UserSeed
    {
        public static async Task Seed(UserManager<User> userManager)
        {
            const string adminID = "d6f46418-2c21-43f8-b167-162fb5e3a999";
            if (await userManager.FindByIdAsync(adminID) == null)
            {
                const string adminEmail = "admin@google.com";
                var adminUser = new User()
                {
                    Id = adminID,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "HonzaAdmin",
                    LastName = "Zvesnice",
                    SystemRoleId = Guid.Parse("e4499f9c-a59c-4b25-a86e-81ca032dc313")
                };

                await userManager.CreateAsync(adminUser, "Admin123");
            }

            const string baseID = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8";
            if (await userManager.FindByIdAsync(baseID) == null)
            {
                const string baseEmail = "pb@google.com";
                var adminUser = new User()
                {
                    Id = baseID,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Pepa",
                    LastName = "Brnak",
                    SystemRoleId = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6")
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseID2 = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2";
            if (await userManager.FindByIdAsync(baseID2) == null)
            {
                const string baseEmail = "zb@google.com";
                var adminUser = new User()
                {
                    Id = baseID2,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Zdenda",
                    LastName = "Branik",
                    SystemRoleId = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6")
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseID3 = "84c8b270-14e5-4158-bcde-a76c6edc4cf7";
            if (await userManager.FindByIdAsync(baseID3) == null)
            {
                const string baseEmail = "ds@google.com";
                var adminUser = new User()
                {
                    Id = baseID3,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "David",
                    LastName = "Sicko",
                    SystemRoleId = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6")
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }

            const string baseID4 = "5862be25-6467-450e-81fa-1cac9578650b";
            if (await userManager.FindByIdAsync(baseID4) == null)
            {
                const string baseEmail = "jk@google.com";
                var adminUser = new User()
                {
                    Id = baseID4,
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true,
                    FirstName = "Jirka",
                    LastName = "Kohout",
                    SystemRoleId = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6")
                };

                await userManager.CreateAsync(adminUser, "Basic123");
            }
        }
    }
}
