using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemRolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SystemRolesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SystemRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemRole>>> GetSystemRoles()
        {
            return await _context.SystemRoles.ToListAsync();
        }

        // GET: api/SystemRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemRole>> GetSystemRole(int id)
        {
            var systemRole = await _context.SystemRoles.FindAsync(id);

            if (systemRole == null)
            {
                return NotFound();
            }

            return systemRole;
        }

        // PUT: api/SystemRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystemRole(int id, SystemRole systemRole)
        {
            if (id != systemRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(systemRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemRoleExists(id))
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

        // POST: api/SystemRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SystemRole>> PostSystemRole(SystemRole systemRole)
        {
            _context.SystemRoles.Add(systemRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSystemRole", new { id = systemRole.Id }, systemRole);
        }

        // DELETE: api/SystemRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemRole(int id)
        {
            var systemRole = await _context.SystemRoles.FindAsync(id);
            if (systemRole == null)
            {
                return NotFound();
            }

            _context.SystemRoles.Remove(systemRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SystemRoleExists(int id)
        {
            return _context.SystemRoles.Any(e => e.Id == id);
        }
    }
}
