using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class ProjectRoleSeed
    {
        public static void Seed(AppDbContext context)
        {
            var rolesToSeed = new ProjectRole[]
            {
                new()
                {
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    // Project manager
                    RoleType = 100, // TODO -> mozna se zmeni kvuli ciselniku
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    RoleType = 50, // Risk manager
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    RoleType = 25, // Project member
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    RoleType = 25, // Extern member
                    IsReqApproved = true
                }
            };

            foreach (var role in rolesToSeed)
            {
                if(!context.ProjectRoles.Any(u => u.UserId == role.UserId && u.RiskProjectId == role.RiskProjectId))
                {
                    context.ProjectRoles.Add(role);
                }
            }
            context.SaveChanges();
        }
    }
}
