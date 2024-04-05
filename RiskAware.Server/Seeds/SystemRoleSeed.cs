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
                    Id = Guid.Parse("e4499f9c-a59c-4b25-a86e-81ca032dc313"),
                    Name = "Admin",
                    IsAdministrator = true,
                    Description = "System admin"
                },
                new()
                {
                    Id = Guid.Parse("8e37b798-5b2f-4ea9-9130-d86ce37c78d6"),
                    Name = "Basic",
                    IsAdministrator = false,
                    Description = "Basic app user"
                }
            };

            foreach (var systemRole in systemRolesToSeed)
            {
                if(!context.SystemRoles.Any(s => s.Id == systemRole.Id))
                {
                    context.SystemRoles.Add(systemRole);
                }
            }
            context.SaveChanges();
        }
    }
}
