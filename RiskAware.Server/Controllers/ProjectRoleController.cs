using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectRoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectRoleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectRole>>> GetProjectRoles()
        {
            return await _context.ProjectRoles.ToListAsync();
        }

        // GET: api/ProjectRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectRole>> GetProjectRole(string id)
        {
            var projectRole = await _context.ProjectRoles.FindAsync(id);

            if (projectRole == null)
            {
                return NotFound();
            }

            return projectRole;
        }

        // PUT: api/ProjectRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectRole(string id, ProjectRole projectRole)
        {
            if (id != projectRole.UserId)
            {
                return BadRequest();
            }

            _context.Entry(projectRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectRole>> PostProjectRole(ProjectRole projectRole)
        {
            _context.ProjectRoles.Add(projectRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectRoleExists(projectRole.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjectRole", new { id = projectRole.UserId }, projectRole);
        }

        // DELETE: api/ProjectRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectRole(string id)
        {
            var projectRole = await _context.ProjectRoles.FindAsync(id);
            if (projectRole == null)
            {
                return NotFound();
            }

            _context.ProjectRoles.Remove(projectRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectRoleExists(string id)
        {
            return _context.ProjectRoles.Any(e => e.UserId == id);
        }
    }
}
