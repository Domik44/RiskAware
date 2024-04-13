using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.Queries
{
    public class ProjectRoleQueries
    {
        private readonly AppDbContext _context;

        public ProjectRoleQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectRoleDto>> GetRiskProjectMembersAsync(int id)
        {
            var projectRoles = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == id)
                .Include(pr => pr.User)
                .Include(pr => pr.ProjectPhase)
                .Select(pr => new ProjectRoleDto
                {
                    Id = pr.Id,
                    RoleName = pr.RoleType.ToString(),
                    IsReqApproved = pr.IsReqApproved,
                    User = new UserDto
                    {
                        Email = pr.User.Email,
                        FullName = pr.User.FirstName + " " + pr.User.LastName
                    },
                    ProjectPhaseName = pr.ProjectPhase.Name
                })
                .ToListAsync();

            return projectRoles;
        }
    }
}
