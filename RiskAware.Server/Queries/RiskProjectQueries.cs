using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.ViewModels;
using System.Linq.Dynamic.Core;

namespace RiskAware.Server.Queries
{
    public class RiskProjectQueries
    {
        private readonly AppDbContext _context;
        private readonly ProjectPhaseQueries _projectPhaseQueries;
        private readonly RiskQueries _riskQueries;
        private readonly ProjectRoleQueries _projectRoleQueries;

        public RiskProjectQueries(AppDbContext context)
        {
            _context = context;
            _projectPhaseQueries = new ProjectPhaseQueries(context);
            _riskQueries = new RiskQueries(context);
            _projectRoleQueries = new ProjectRoleQueries(context);
        }

        private static DateTime ParseClientDate(string date, DateTime defaultValue)
        {
            return DateTime.TryParse(date, out DateTime parsed) ? parsed : defaultValue;
        }

        public IQueryable<RiskProjectDto> ApplyFilterQueryProjects(
            IQueryable<RiskProjectDto> query, DtParams dtParams)
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
                        query.Where(p => p.Start >= ParseClientDate(filter.Value, DateTime.MinValue)),  // datetime property
                    nameof(RiskProjectDto.End) =>
                        query.Where(p => p.End <= ParseClientDate(filter.Value, DateTime.MaxValue)),
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
                query = query.OrderBy($"{sorting.Id} {sorting.Dir}");
            }
            return query;
        }

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
                        NumOfMembers = u.ProjectRoles.Count,
                        ProjectManagerName = u.ProjectRoles
                            .Where(pr => pr.RoleType == RoleType.ProjectManager)
                            .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                            .FirstOrDefault()
                    }
                );
        }

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
                    NumOfMembers = projects.ProjectRoles.Count,
                    ProjectManagerName = projects.ProjectRoles
                        .Where(pr => pr.RoleType == RoleType.ProjectManager)
                        .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                        .FirstOrDefault()
                });

            // TODO -> seems to be faster
            //return from projectRole in _context.ProjectRoles
            //            where projectRole.UserId == user.Id
            //            join riskProject in _context.RiskProjects on projectRole.RiskProjectId equals riskProject.Id
            //            select new RiskProjectDto//(riskProject);
            //            {
            //                Id = riskProject.Id,
            //                Title = riskProject.Title,
            //                Start = riskProject.Start,
            //                End = riskProject.End,
            //                NumOfMembers = riskProject.ProjectRoles.Count,
            //                ProjectManagerName = riskProject.ProjectRoles
            //                    .Where(pr => pr.RoleType == RoleType.ProjectManager)
            //                    .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
            //                    .FirstOrDefault()
            //            };
        }

        public async Task<IEnumerable<RiskProjectDto>> GetAllAdminRiskProjectsAsync(User user)
        {
            var projects = await _context.RiskProjects
                .AsNoTracking()
                .Where(rp => rp.UserId == user.Id)
                .Select(rp => new RiskProjectDto
                {
                    Id = rp.Id,
                    Title = rp.Title,
                    Start = rp.Start,
                    End = rp.End,
                    NumOfMembers = rp.ProjectRoles.Count,
                    ProjectManagerName = rp.ProjectRoles
                        .Where(pr => pr.RoleType == RoleType.ProjectManager)
                        .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return projects;
        }

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

        public async Task<RiskProjectPageDto> GetRiskProjectPageAsync(int id, string userId)
        {
            var detail = await GetRiskProjectDetailAsync(id);

            if (detail == null)
            {
                return null;
            }

            var phases = await _projectPhaseQueries.GetRiskProjectPhasesAsync(id);
            var risks = await _riskQueries.GetRiskProjectRisksAsync(id);
            var members = await _projectRoleQueries.GetRiskProjectMembersAsync(id);
            var userRole = await _projectRoleQueries.GetUsersRoleOnRiskProjectAsync(id, userId);

            return new RiskProjectPageDto
            {
                Detail = detail,
                Phases = phases,
                Risks = risks,
                Members = members,
                UserRole = userRole
            };
        }

        public async Task<bool> IsProjectManager(RiskProject riskProject, User user)
        {
            return await _context.ProjectRoles
                .AsNoTracking()
                .AnyAsync(pr => pr.RiskProjectId == riskProject.Id && pr.UserId == user.Id && pr.RoleType == RoleType.ProjectManager);
        }

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
    }
}
