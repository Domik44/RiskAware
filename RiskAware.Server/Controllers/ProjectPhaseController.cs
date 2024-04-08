using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPhaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectPhaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectPhases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectPhase>>> GetProjectPhases()
        {
            return await _context.ProjectPhases.ToListAsync();
        }

        // GET: api/ProjectPhases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectPhase>> GetProjectPhase(Guid id)
        {
            var projectPhase = await _context.ProjectPhases.FindAsync(id);

            if (projectPhase == null)
            {
                return NotFound();
            }

            return projectPhase;
        }

        // PUT: api/ProjectPhases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectPhase(Guid id, ProjectPhase projectPhase)
        {
            if (id != projectPhase.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectPhase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectPhaseExists(id))
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

        // POST: api/ProjectPhases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectPhase>> PostProjectPhase(ProjectPhase projectPhase)
        {
            _context.ProjectPhases.Add(projectPhase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectPhase", new { id = projectPhase.Id }, projectPhase);
        }

        // DELETE: api/ProjectPhases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectPhase(Guid id)
        {
            var projectPhase = await _context.ProjectPhases.FindAsync(id);
            if (projectPhase == null)
            {
                return NotFound();
            }

            _context.ProjectPhases.Remove(projectPhase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectPhaseExists(Guid id)
        {
            return _context.ProjectPhases.Any(e => e.Id == id);
        }
    }
}
