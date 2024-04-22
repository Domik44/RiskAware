using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using System.Linq.Dynamic.Core;

namespace RiskAware.Server.Queries
{
    /// <summary>
    /// Class containing queries for project roles.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class ProjectRoleQueries
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for ProjectRoleQueries.
        /// </summary>
        /// <param name="context"> Application DB context. </param>
        public ProjectRoleQueries(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for getting project roles of a risk project with the given id.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Returns collection of DTOs containing basic info about project role. </returns>
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

        /// <summary>
        /// Query for getting project roles of a risk project with the given id.
        /// Filtered by the given datatable parameters.
        /// </summary>
        /// <param name="projectId"> Id of risk project. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns collection of DTOs containing basic info about project role. </returns>
        public IQueryable<ProjectRoleListDto> QueryRiskProjectMembers(int projectId, DtParamsDto dtParams)
        {
            var query = _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == projectId)
                .Include(pr => pr.User)
                .Include(pr => pr.ProjectPhase)
                .Select(pr => new ProjectRoleListDto
                {
                    Id = pr.Id,
                    Fullname = pr.User.FirstName + " " + pr.User.LastName,
                    Email = pr.User.Email,
                    RoleName = pr.Name,
                    ProjectPhaseName = pr.ProjectPhase.Name,
                    IsReqApproved = pr.IsReqApproved,
                });

            foreach (var filter in dtParams.Filters)
            {
                query = filter.PropertyName switch
                {
                    nameof(ProjectRoleDto.User.FullName) =>
                        query.Where(r => r.Fullname.StartsWith(filter.Value)),
                    nameof(ProjectRoleDto.User.Email) =>
                        query.Where(r => r.Email.StartsWith(filter.Value)),
                    nameof(ProjectRoleDto.RoleName) =>
                        query.Where(r => r.RoleName.StartsWith(filter.Value)),
                    nameof(ProjectRoleDto.ProjectPhaseName) =>
                        query.Where(r => r.ProjectPhaseName.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter
                };
            }

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                query = query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(r => r.Id);
            }
            else
            {
                query = query.OrderByDescending(r => r.Id);
            }
            return query;
        }

        public async Task<bool> HasProjectRoleOnRiskProject(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId);
        }

        /// <summary>
        /// This method checks if has a role as an project manager on a project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Return true if user is project manager. </returns>
        public async Task<bool> IsProjectManager(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.ProjectManager);
        }

        /// <summary>
        /// This method checks if has a role as an risk manager on a project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Return true if user is risk manager. </returns>
        public async Task<bool> IsRiskManager(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.RiskManager);
        }

        /// <summary>
        /// This method checks if has a role as an team member on a project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Return true if user is team member. </returns>
        public async Task<bool> IsTeamMember(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.TeamMember);
        }

        /// <summary>
        /// This method checks if has a role as an extern member on a project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Return true if user is extern member. </returns>
        public async Task<bool> IsExternMember(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.RoleType == RoleType.ExternalMember);
        }

        /// <summary>
        /// This method checks if user is just looking at the project without any role on it.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Return true if user is just a looker. </returns>
        public async Task<bool> IsCommonUser(int riskProjectId, string userId)
        {
            var hasRole = await HasProjectRoleOnRiskProject(riskProjectId, userId);
            if (hasRole)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method for checking if user is assigned to the phase.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="phaseId"> Id of phase. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns></returns>
        public async Task<bool> IsAssignedToPhase(int riskProjectId, int phaseId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && pr.ProjectPhaseId == phaseId);
        }

        /// <summary>
        /// Method for checking if user is allowed to approve/decline risk.
        /// </summary>
        /// <param name="riskId"> Id of risk. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns></returns>
        public async Task<bool> IsAllowedToDeclineApproveRisk(int riskId, string userId)
        {
            var isProjectManager = await IsProjectManager(riskId, userId);
            var isRiskManager = await IsRiskManager(riskId, userId);

            if (isProjectManager || isRiskManager)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for checking if user is allowed to add/edit risk.
        /// </summary>
        /// <param name="riskId"> Id of risk. </param>
        /// <param name="phaseId"> Id of phase we are adding/editing to. </param>
        /// <param name="userId"> Id of user adding/editing risk. </param>
        /// <returns> Returns true if user is allowed to add/edit. </returns>
        public async Task<bool> IsAllowedToAddEditRisk(int riskId, int phaseId, string userId)
        {
            var isProjectManager = await IsProjectManager(riskId, userId);
            var isRiskManager = await IsRiskManager(riskId, userId);
            var isTeamMember = await IsTeamMember(riskId, userId);
            var isAssignedToPhase = await IsAssignedToPhase(riskId, phaseId, userId);

            if (isProjectManager || isRiskManager)
            {
                return true;
            }

            if (isTeamMember && isAssignedToPhase)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for checking if user is allowed to delete/restore risk.
        /// </summary>
        /// <param name="risk"> Entity representing risk. </param>
        /// <param name="userId"> Id of user trying to delete/restore risk. </param>
        /// <returns> Returns true if user can delete/restore risk. </returns>
        public async Task<bool> IsAllowedToDeleteRestoreRisk(Risk risk, string userId)
        {
            var isProjectManager = await IsProjectManager(risk.RiskProjectId, userId);
            var isRiskManager = await IsRiskManager(risk.RiskProjectId, userId);

            if (isProjectManager || isRiskManager)
            {
                return true;
            }

            if(risk.UserId == userId)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for checking if user has basic editing premissions on certain risk project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns true if user has basic edit premissions. </returns>
        public async Task<bool> HasBasicEditPermissions(int riskProjectId, string userId)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId && (pr.RoleType == RoleType.ProjectManager || pr.RoleType == RoleType.RiskManager || pr.RoleType == RoleType.TeamMember));
        }

        /// <summary>
        /// Method for getting what role user has on a risk project.
        /// What RoleTypes see RoleType enum in Models.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> Id of user, which role we want to get. </param>
        /// <returns> Return what type of role user has on project. </returns>
        public async Task<RoleType> GetUsersRoleOnRiskProjectAsync(int riskProjectId, string userId)
        {
            var role = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == riskProjectId && pr.UserId == userId)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return RoleType.CommonUser;
            }

            return role.RoleType;
        }

        /// <summary>
        /// Method for getting what phase user is assigned to on a risk project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="userId"> id of user. </param>
        /// <returns> Returns DTO containing basic info about assigned project phase, or null in case of no phase assigned or no role. </returns>
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
