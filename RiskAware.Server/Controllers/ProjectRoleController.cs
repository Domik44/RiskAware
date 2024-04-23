using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    /// <summary>
    /// Controller for handling project role related requests.
    /// </summary>
    /// <author>Dominik Pop</author>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectRoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ProjectRoleQueries _projectRoleQueries;
        private readonly RiskProjectQueries _riskProjectQueries;

        public ProjectRoleController(AppDbContext context, UserManager<User> userManager, ProjectRoleQueries projectRoleQueries, RiskProjectQueries riskProjectQueries)
        {
            _context = context;
            _userManager = userManager;
            _projectRoleQueries = projectRoleQueries;
            _riskProjectQueries = riskProjectQueries;
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// Get filtered members working on the project.
        /// </summary>
        /// <param name="dtParams"> Data table filtering parameters. </param>
        /// <returns> Filtered member DTOs. </returns>
        [HttpPost("/api/RiskProject/{projectId}/Members")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjectMembers(int projectId, [FromBody] DtParamsDto dtParams)
        {
            var query = _projectRoleQueries.QueryRiskProjectMembers(projectId, dtParams);
            int totalRowCount = await query.CountAsync();
            var members = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResultDto<ProjectRoleListDto>
            {
                Data = members,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for adding user to the project with a role.
        /// </summary>
        /// <param name="riskProjectId"> Id of a risk project. </param>
        /// <param name="projectRoleDto"> DTO containing info about new project role. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        [HttpPost("/api/RiskProject/{riskProjectId}/AddUserToRiskProject")]
        public async Task<IActionResult> AddUserToRiskProject(int riskProjectId, ProjectRoleCreateDto projectRoleDto)
        {
            var activeUser = await _userManager.GetUserAsync(User);

            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found!");
            }

            var isProjectManager = await _projectRoleQueries.IsProjectManager(riskProject.Id, activeUser.Id);
            var isRiskManager = await _projectRoleQueries.IsRiskManager(riskProject.Id, activeUser.Id);
            if (!isProjectManager && !isRiskManager)
            {
                return Unauthorized("User is not authorized for this action!");
            }

            var userToBeAdded = await _context.Users.Where(u => u.Email == projectRoleDto.Email).FirstOrDefaultAsync();
            if (userToBeAdded == null)
            {
                return NotFound("User not found");
            }

            var hasProjectRole = await _projectRoleQueries.HasProjectRoleOnRiskProject(riskProjectId, userToBeAdded.Id);
            if (hasProjectRole)
            {
                return BadRequest("User is already a member of this project");
            }

            var newProjectRole = new ProjectRole
            {
                UserId = userToBeAdded.Id,
                RiskProjectId = riskProjectId,
                RoleType = projectRoleDto.RoleType,
                IsReqApproved = true,
                Name = projectRoleDto.Name
            };

            if (projectRoleDto.ProjectPhaseId != null)
            {
                var projectPhase = await _context.ProjectPhases.Where(pp => pp.Id == projectRoleDto.ProjectPhaseId).FirstOrDefaultAsync();
                if (projectPhase == null)
                {
                    return NotFound("Project phase not found");
                }
                newProjectRole.ProjectPhaseId = projectPhase.Id;
            }

            _context.ProjectRoles.Add(newProjectRole);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        /// <summary>
        /// Method for assigning project phase to project role.
        /// </summary>
        /// <param name="projectPhaseId"> Id of project phase. </param>
        /// <param name="projectRoleId"> Id of project role. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("/api/ProjectRole/{projectRoleId}/AssignPhase/{projectPhaseId}")] 
        public async Task<IActionResult> AssignPhaseToUser(int projectPhaseId, int projectRoleId) // TODO -> implement
        {
            var projectRole = await _context.ProjectRoles.Where(pr => pr.Id == projectRoleId).FirstOrDefaultAsync();
            if (projectRole == null)
            {
                return NotFound("Project role not found");
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.Id == projectPhaseId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return NotFound("Project phase not found");
            }

            projectRole.ProjectPhaseId = projectPhase.Id;
            await _context.SaveChangesAsync();

            return Ok();
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> ChangeRiskProjectUserRoleType(int riskProjectId, ProjectRoleCreateDto projectRoleDto) // TODO -> implement
        //{
              // TODO implement
        //    return Ok();
        //}

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// Method for removing user from the project.
        /// </summary>
        /// <param name="projectRoleId"> Id of project role. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpDelete("{projectRoleId}")]
        public async Task<IActionResult> RemoveUserFromRiskProject(int projectRoleId)
        {
            var role = await _context.ProjectRoles.FindAsync(projectRoleId);
            if (role == null)
            {
                return NotFound("Project role not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(role.RiskProjectId, activeUser.Id);
            if (!isProjectManager)
            {
                return Unauthorized("User doesnt have premissions!");
            }

            if(activeUser.Id == role.UserId)
            {
                return BadRequest("Nemůžete odstranit sebe sama z projektu!"); // msg used in frontend
            }

            _context.ProjectRoles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
