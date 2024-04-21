using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using System.Linq.Dynamic.Core;

namespace RiskAware.Server.Queries
{
    /// <summary>
    /// Class containing queries for RiskProject entity.
    /// </summary>
    /// <param name="context"> Application DB context. </param>
    /// <author> Dominik Pop </author>
    public class RiskProjectQueries(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        private readonly ProjectPhaseQueries _projectPhaseQueries = new ProjectPhaseQueries(context);
        private readonly RiskQueries _riskQueries = new RiskQueries(context);
        private readonly ProjectRoleQueries _projectRoleQueries = new ProjectRoleQueries(context);

        /// <summary>
        /// Query for filtering and sorting projects.
        /// </summary>
        /// <param name="query"> Input query. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns collection of DTOs containing basic info about risk project. </returns>
        public IQueryable<RiskProjectDto> ApplyFilterQueryProjects(
            IQueryable<RiskProjectDto> query, DtParamsDto dtParams)
        {
            foreach (var filter in dtParams.Filters)
            {
                // todo string properties can be filtered by Contains or StartsWith
                query = filter.PropertyName switch
                {
                    nameof(RiskProjectDto.Id) =>
                        query.Where(p => p.Id.ToString().StartsWith(filter.Value)), // numeric property
                    nameof(RiskProjectDto.Title) =>
                        query.Where(p => p.Title.StartsWith(filter.Value)),         // string property
                    nameof(RiskProjectDto.Start) =>
                        query.Where(p => p.Start >= DtParamsDto
                            .ParseClientDate(filter.Value, DateTime.MinValue)),     // datetime property
                    nameof(RiskProjectDto.End) =>
                        query.Where(p => p.End <= DtParamsDto
                            .ParseClientDate(filter.Value, DateTime.MaxValue)),
                    nameof(RiskProjectDto.NumOfMembers) =>
                        query.Where(p => p.NumOfMembers.ToString().StartsWith(filter.Value)),
                    nameof(RiskProjectDto.ProjectManagerName) =>
                        query.Where(p => p.ProjectManagerName.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter
                };
            }

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                query = query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(p => p.Id);
            }
            else
            {
                query = query.OrderByDescending(p => p.Id);
            }
            return query;
        }

        /// <summary>
        /// Query for getting all risk projects in the system.
        /// </summary>
        /// <returns> Collection of DTOs containing basic info about risk project. </returns>
        public IQueryable<RiskProjectDto> QueryAllProjects()
        {
            return _context.RiskProjects
                .AsNoTracking()
                .Select(u =>
                    new RiskProjectDto
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Start = u.Start,
                        End = u.End,
                        IsValid = u.IsValid,
                        NumOfMembers = u.ProjectRoles.Count,
                        ProjectManagerName = u.ProjectRoles
                            .Where(pr => pr.RoleType == RoleType.ProjectManager)
                            .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                            .FirstOrDefault()
                    }
                );
        }

        /// <summary>
        /// Query for getting all risk projects of a logged user.
        /// </summary>
        /// <param name="user"> User whos projects we want to get. </param>
        /// <returns> Collection of DTOs containing basic info about risk project. </returns>
        public IQueryable<RiskProjectDto> QueryUsersProjects(User user)
        {
            return _context.RiskProjects
                .AsNoTracking()
                .Where(rp => rp.ProjectRoles.Any(pr => pr.UserId == user.Id))
                .Select(projects => new RiskProjectDto
                {
                    Id = projects.Id,
                    Title = projects.Title,
                    Start = projects.Start,
                    End = projects.End,
                    IsValid = projects.IsValid,
                    NumOfMembers = projects.ProjectRoles.Count,
                    ProjectManagerName = projects.ProjectRoles
                        .Where(pr => pr.RoleType == RoleType.ProjectManager)
                        .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                        .FirstOrDefault()
                });
        }

        /// <summary>
        /// Query for getting a risk project detail.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Returns DTO containing basic detail about risk project. </returns>
        public async Task<RiskProjectDetailDto> GetRiskProjectDetailAsync(int id)
        {
            var comments = await GetRiskProjectCommentsAsync(id);
            var riskProject = await _context.RiskProjects
                .AsNoTracking()
                .Where(pr => pr.Id == id)
                .Include(pr => pr.Comments)
                .ThenInclude(c => c.User)
                .Select(pr => new RiskProjectDetailDto
                {
                    Id = pr.Id,
                    Title = pr.Title,
                    Description = pr.Description ?? "",
                    Start = pr.Start,
                    End = pr.End,
                    Comments = comments,
                    IsBlank = pr.IsBlank,
                    Scale = pr.Scale,
                })
                .FirstOrDefaultAsync();

            if (riskProject == null)
            {
                return null;
            }

            return riskProject;
        }

        /// <summary>
        /// Query for getting a risk project page.
        /// It includes all necessary data for the page.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <param name="userId"> Id of logged user. </param>
        /// <returns> Returns DTO containing all possible information associated to a risk project. </returns>
        public async Task<RiskProjectPageDto> GetRiskProjectPageAsync(int id, string userId)
        {
            var detail = await GetRiskProjectDetailAsync(id);

            if (detail == null)
            {
                return null;
            }

            // Queries for all nav tabs.
            var phases = await _projectPhaseQueries.GetRiskProjectPhasesAsync(id);
            var risks = await _riskQueries.GetRiskProjectRisksAsync(id);
            var members = await _projectRoleQueries.GetRiskProjectMembersAsync(id);

            // Query for getting role of logged user on the project.
            var userRole = await _projectRoleQueries.GetUsersRoleOnRiskProjectAsync(id, userId);

            // Query for getting assigned phase of logged user on the project.
            var assignedPhase = await _projectRoleQueries.GetUsersAssignedPhaseAsync(id, userId);

            return new RiskProjectPageDto
            {
                Detail = detail,
                Phases = phases,
                Risks = risks,
                Members = members,
                UserRole = userRole,
                AssignedPhase = assignedPhase
            };
        }

        /// <summary>
        /// Query for getting all comments posted on a risk project.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Returns collection of DTOs containing comment info. </returns>
        public async Task<IEnumerable<CommentDto>> GetRiskProjectCommentsAsync(int id)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Where(c => c.RiskProjectId == id)
                .Include(c => c.User)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    Created = c.Created,
                    Author = c.User.FirstName + " " + c.User.LastName
                })
                .OrderByDescending(c => c.Created)
                .ToListAsync();

            return comments;
        }

        /// <summary>
        /// Query for getting all risk categories of a risk project.
        /// </summary>
        /// <param name="riskProject"> Risk project entity. </param>
        /// <returns> Returns true if creation was successful. </returns>
        public async Task<bool> CreateDefaultCategories(RiskProject riskProject) {
            ICollection<string> names = [ "Finanční rizika","Lidská rizika","Operační rizika","Legislativní rizika","Technická rizika"];
            foreach (var name in names)
            {
                var newRiskCategory = new RiskCategory
                {
                    Name = name,
                    RiskProjectId = riskProject.Id
                };
                _context.RiskCategories.Add(newRiskCategory);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
