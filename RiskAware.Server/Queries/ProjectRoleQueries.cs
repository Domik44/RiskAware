using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;

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
            var projectRoles = await _context.ProjectRoles // TODO -> fix missing projectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == id)
                .Include(pr => pr.User)
                .Select(pr => new ProjectRoleDto
                {
                    Id = pr.Id,
                    RoleName = pr.RoleType.ToString(),
                    IsReqApproved = pr.IsReqApproved,
                    User = new UserDto
                    {
                        Email = pr.User.Email,
                        FullName = pr.User.FirstName + " " + pr.User.LastName
                    }
                })
                .ToListAsync();

            return projectRoles;
        }
    }
}
