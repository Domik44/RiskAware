using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class SystemRoleSeed
    {
        public static void Seed(AppDbContext context)
        {
            var systemRolesToSeed = new SystemRole[]
            {
                new()
                {
                    Name = "Admin",
                    IsAdministrator = true,
                    Description = "System admin"
                },
                new()
                {
                    Name = "Basic",
                    IsAdministrator = false,
                    Description = "Basic app user"
                }
            };

            foreach (var systemRole in systemRolesToSeed)
            {
                if (!context.SystemRoles.Any(s => s.Id == systemRole.Id))
                {
                    context.SystemRoles.Add(systemRole);
                }
            }
            context.SaveChanges();
        }
    }
}
