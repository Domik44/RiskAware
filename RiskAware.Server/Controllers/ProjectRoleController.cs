using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectRoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ProjectRoleQueries _projectRoleQueries;

        public ProjectRoleController(AppDbContext context, ProjectRoleQueries projectRoleQueries)
        {
            _context = context;
            _projectRoleQueries = projectRoleQueries;
        }

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// This controller method returns all members of a specific project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns> Returns DTO for risk project members tab. </returns>
        /// url: api/ProjectRole/5/Members
        [HttpGet("/api/RiskProject/{id}/Members")] // TODO -> mby specify route so it makes more sense -> like api/RiskProject/5/Members ??
        public async Task<ActionResult<IEnumerable<ProjectRoleDto>>> GetUsersOnRiskProject(int id)
        {
            // i need to get all project roles for a certain risk project and then get all users from that project roles and then get phases for each project role
            // TODO -> add navigation property to ProjectRole for ProjectPhase so I can include it as well
            var projectRoles = await _projectRoleQueries.GetRiskProjectMembersAsync(id);

            return Ok(projectRoles);
        }

        ////////////////// POST METHODS //////////////////
        
        // POST: api/ProjectRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectRole>> AddUserToRiskProject(ProjectRole projectRole)
        {
            // TODO -> implement this method
            //_context.ProjectRoles.Add(projectRole);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (ProjectRoleExists(projectRole.UserId))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetProjectRole", new { id = projectRole.UserId }, projectRole);
            return null;
        }

        ////////////////// PUT METHODS //////////////////
        
        // PUT: api/ProjectRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeRiskProjectUserRoleType(string id, ProjectRole projectRole)
        {
            // TODO -> implement this method
            //if (id != projectRole.UserId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(projectRole).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProjectRoleExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return null;
        }

        ////////////////// DELETE METHODS //////////////////
        
        // DELETE: api/ProjectRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUserFromRiskProject(string id)
        {
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

        private bool ProjectRoleExists(string id)
        {
            return _context.ProjectRoles.Any(e => e.UserId == id);
        }
    }
}
