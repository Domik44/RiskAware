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
                    RiskProjectId = 1,
                    Name = RoleType.ProjectManager.ToString(),
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    RiskProjectId = 1,
                    Name = RoleType.RiskManager.ToString(),
                    RoleType = RoleType.RiskManager,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 1,
                    Name = "Analytik",
                    RoleType = RoleType.TeamMember,
                    IsReqApproved = true,
                    ProjectPhaseId = 1
                },
                new()
                {
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    Name = "Analytik",
                    RoleType = RoleType.TeamMember,
                    IsReqApproved = true,
                    ProjectPhaseId = 2
                },
                new()
                {
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    RiskProjectId = 2,
                    Name = RoleType.ProjectManager.ToString(),
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 3,
                    Name = RoleType.ProjectManager.ToString(),
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true,
                    ProjectPhaseId = 1
                },
                new()
                {
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    RiskProjectId = 3,
                    Name = "Analytik",
                    RoleType = RoleType.TeamMember,
                    IsReqApproved = true,
                    ProjectPhaseId = 5
                },
            };

            foreach (var role in rolesToSeed)
            {
                if (!context.ProjectRoles.Any(p => p.Id == role.Id))
                {
                    context.ProjectRoles.Add(role);
                }
            }
            context.SaveChanges();
        }
    }
}
