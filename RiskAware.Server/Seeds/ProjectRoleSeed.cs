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
                    Name = "Projektový manažer",
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    RiskProjectId = 1,
                    Name = "Rizikový manažer",
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
                    Name = "Návrhář",
                    RoleType = RoleType.TeamMember,
                    IsReqApproved = true,
                    ProjectPhaseId = 2
                },
                new()
                {
                    UserId = "31ab7787-60dc-4309-a069-4c30fc837ef0",
                    RiskProjectId = 1,
                    Name = "Externí člen",
                    RoleType = RoleType.ExternalMember,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "12749a7a-100b-4e69-a234-7d059e508d5b",
                    RiskProjectId = 1,
                    Name = "Programátor",
                    RoleType = RoleType.TeamMember,
                    IsReqApproved = true,
                    ProjectPhaseId = 5
                },
                new()
                {
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    RiskProjectId = 2,
                    Name = "Projektový manažer",
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
                },
                new()
                {
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 3,
                    Name = "Projektový manažer",
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
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
                new()
                {
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 4,
                    Name = "Projektový manažer",
                    RoleType = RoleType.ProjectManager,
                    IsReqApproved = true
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
