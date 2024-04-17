using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;

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
                    RoleName = pr.Name,
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

        public async Task<bool> HasProjectRoleOnRiskProject(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId);
        }

        public async Task<bool> IsProjectManager(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.ProjectManager);
        }

        public async Task<bool> IsRiskManager(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.RiskManager);
        }

        public async Task<bool> HasBasicEditPermissions(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && (pr.RoleType == RoleType.ProjectManager || pr.RoleType == RoleType.RiskManager || pr.RoleType == RoleType.TeamMember));
        }

        public async Task<RoleType> GetUsersRoleOnRiskProjectAsync(int riskProjectId, string userId)
        {
            var role = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId)
                //.Select(pr => pr.RoleType)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return RoleType.CommonUser;
            }

            return role.RoleType;
        }

        public async Task<ProjectPhaseSimpleDto> GetUsersAssignedPhaseAsync(int riskProjectId, string userId)
        {
            var role = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId)
                .Include(pr => pr.ProjectPhase)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return null;
            }

            if (role.ProjectPhase == null)
            {
                return null;
            }

            return new ProjectPhaseSimpleDto
            {
                Id = role.ProjectPhase.Id,
                Name = role.ProjectPhase.Name
            };
        }
    }
}
