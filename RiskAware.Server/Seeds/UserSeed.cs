using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class UserSeed
    {
        public static async Task Seed(UserManager<User> userManager)
        {
            var usersToSeed = new User[]
            {
                new()
                {
                    UserId = Guid.Parse("d6f46418-2c21-43f8-b167-162fb5e3a999"),
                    FirstName = "Honza",
                    LastName = "Zvesnice",
                    Email = "hz@google.com"
                },
                new()
                {
                    UserId = Guid.Parse("39123a3c-3ce3-4bcc-8887-eb7d8e975ea8"),
                    FirstName = "Pepa",
                    LastName = "Brnak",
                    Email = "pb@google.com"
                }
            };

            foreach (var user in usersToSeed)
            {
                await userManager.CreateAsync(user, "Heslo123");
            }
        }
    }
}
