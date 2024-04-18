﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
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

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// This controller method returns all members of a specific project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns> Returns DTO for risk project members tab. </returns>
        /// url: api/ProjectRole/5/Members
        [HttpGet("/api/RiskProject/{id}/Members")]
        public async Task<ActionResult<IEnumerable<ProjectRoleDto>>> GetUsersOnRiskProject(int id)
        {
            var projectRoles = await _projectRoleQueries.GetRiskProjectMembersAsync(id);
            return Ok(projectRoles);
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// Get filtered members working on the project
        /// </summary>
        /// <param name="dtParams">Data table filtering parameters</param>
        /// <returns>Filtered member DTOs</returns>
        [HttpPost("/api/RiskProject/{projectId}/Members")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjects(int projectId, [FromBody] DtParamsDto dtParams)
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

        // POST: api/ProjectRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/RiskProject/{riskProjectId}/AddUserToRiskProject")]
        public async Task<IActionResult> AddUserToRiskProject(int riskProjectId, ProjectRoleCreateDto projectRoleDto) // TODO -> think about the return type of this -> mby change when frontend is being implemented
        {
            var activeUser = await _userManager.GetUserAsync(User);

            // get project and check if it was found
            // TODO -> think more about how this could be attacked
            // TODO -> mby just check if it exists? 
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            // check if the activeUser is a project manager
            //if(!_riskProjectQueries.IsProjectManager(riskProject, activeUser).Result) // TODO -> could be attacked like this??
            if (projectRoleDto.UserRoleType != RoleType.ProjectManager)
            {
                return Unauthorized();
            }

            // TODO -> check if user is already a member of the project
            // if so -> return BadRequest
            var userToBeAdded = await _context.Users.Where(u => u.Email == projectRoleDto.Email).FirstOrDefaultAsync();
            if (userToBeAdded == null)
            {
                return NotFound("User not found");
            }

            if (_projectRoleQueries.HasProjectRoleOnRiskProject(riskProjectId, userToBeAdded.Id).Result)
            {
                return BadRequest("User is already a member of this project");
            }

            // create new projectRole for this project based on give DTO
            var newProjectRole = new ProjectRole
            {
                UserId = userToBeAdded.Id,
                RiskProjectId = riskProjectId,
                RoleType = projectRoleDto.RoleType,
                IsReqApproved = true,
                Name = projectRoleDto.Name
            };

            // check if projectPhase was given and if yes set assign newly added user to this phase
            if (projectRoleDto.ProjectPhaseId != null) // TODO -> mby think of better way to do this
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

        [HttpPost("/api/RiskProject/{riskProjectId}/JoinRequest")]
        public async Task<IActionResult> CreateJoinRequest(int riskProjectId)
        {
            // TODO -> should be checked if user is already a member of the project
            // if so -> return BadRequest
            var activeUser = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            if (_projectRoleQueries.HasProjectRoleOnRiskProject(riskProjectId, activeUser.Id).Result)
            {
                return BadRequest("User is already a member of this project");
            }

            var newProjectRole = new ProjectRole
            {
                UserId = activeUser.Id,
                RiskProjectId = riskProjectId,
                RoleType = RoleType.CommonUser,
                IsReqApproved = false,
                Name = RoleType.CommonUser.ToString()
            };

            _context.ProjectRoles.Add(newProjectRole);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/ProjectRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> ChangeRiskProjectUserRoleType(int riskProjectId, ProjectRoleCreateDto projectRoleDto)
        //{
        //    // check if active User is a project manager on given project
        //    var activeUser = await _userManager.GetUserAsync(User);
        //    var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
        //    if(riskProject == null)
        //    {
        //        return NotFound("Risk project not found");
        //    }

        //    if(!_riskProjectQueries.IsProjectManager(riskProject, activeUser).Result)
        //    {
        //        return Unauthorized();
        //    }


        //    // get projectRole we want to change from db and check if it exists
        //    var projectRole = await _context.ProjectRoles.Where(pr => pr.UserId == projectRoleDto.UserId && pr.RiskProjectId == riskProjectId).FirstOrDefaultAsync();
        //    if(projectRole == null)
        //    {
        //        return NotFound();
        //    }

        //    if(projectRoleDto.RoleType != RoleType.TeamMember)
        //    {
        //        // in this case we check if user has assigned phase to him
        //        // if so we need to de-assign him from this phase
        //        var projectPhase = await _context.ProjectPhases.Where(pp => pp.RiskProjectId == riskProjectId && pp.ProjectRoleId == projectRole.Id).FirstOrDefaultAsync();
        //        if (projectPhase != null)
        //        {
        //            projectPhase.ProjectRoleId = null;
        //        }
        //    }

        //    projectRole.RoleType = projectRoleDto.RoleType;
        //    projectRole.Name = projectRoleDto.Name;
        //    _context.SaveChanges();

        //    return Ok();
        //}

        [HttpPut("/api/RiskProject/{riskProjectId}/ApproveJoinRequest/{projectRoleId}")]
        public async Task<IActionResult> ApproveJoinRequest(int riskProjectId, int projectRoleId)
        {
            // this approval will be done in members table in frontend
            // this method will be called when project manager approves the request
            // and the member will stay as external member until he is assigned to some higher role
            var activeUser = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project doesnt exist");
            }

            if (!_riskProjectQueries.IsProjectManager(riskProject, activeUser).Result)
            {
                return Unauthorized();
            }

            var projectRole = await _context.ProjectRoles.Where(pr => pr.Id == projectRoleId).FirstOrDefaultAsync();
            if (projectRole == null)
            {
                return NotFound("Request doesnt exist");
            }

            projectRole.IsReqApproved = true;
            _context.SaveChanges();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/ProjectRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUserFromRiskProject(string id)
        {
            // this logic will be harder
            // will have to check if user we want to remove has created some risks/other stuff on the project
            // if so -> we will have to reassign them to someone else
            // if not -> we can just remove user from the project

            // TODO -> implement this method
            //var projectRole = await _context.ProjectRoles.FindAsync(id);
            //if (projectRole == null)
            //{
            //    return NotFound();
            //}

            //_context.ProjectRoles.Remove(projectRole);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return null;
        }

        [HttpDelete("/api/RiskProject/{riskProjectId}/DeclineJoinRequest/{projectRoleId}")]
        public async Task<IActionResult> DeclineJoinRequest(int riskProjectId, int projectRoleId)
        {
            // this method will be called when project manager declines the request from the members table
            // and the user will be removed from the project
            var activeUser = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project doesnt exist");
            }

            if (!_riskProjectQueries.IsProjectManager(riskProject, activeUser).Result)
            {
                return Unauthorized();
            }

            var projectRole = await _context.ProjectRoles.Where(pr => pr.Id == projectRoleId).FirstOrDefaultAsync();
            if (projectRole == null)
            {
                return NotFound("Request doesnt exist");
            }

            _context.ProjectRoles.Remove(projectRole);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProjectRoleExists(string id)
        {
            return _context.ProjectRoles.Any(e => e.UserId == id);
        }
    }
}
