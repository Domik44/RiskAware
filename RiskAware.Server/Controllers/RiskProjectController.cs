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
    public class RiskProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RiskProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RiskProject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RiskProject>>> GetRiskProjects()
        {
            return await _context.RiskProjects.ToListAsync();
        }

        // GET: api/RiskProject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskProject>> GetRiskProject(Guid id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);

            if (riskProject == null)
            {
                return NotFound();
            }

            return riskProject;
        }

        // PUT: api/RiskProject/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRiskProject(Guid id, RiskProject riskProject)
        {
            if (id != riskProject.Id)
            {
                return BadRequest();
            }

            _context.Entry(riskProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiskProjectExists(id))
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

        // POST: api/RiskProject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RiskProject>> PostRiskProject(RiskProject riskProject)
        {
            _context.RiskProjects.Add(riskProject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRiskProject", new { id = riskProject.Id }, riskProject);
        }

        // DELETE: api/RiskProject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskProject(Guid id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }

            _context.RiskProjects.Remove(riskProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RiskProjectExists(Guid id)
        {
            return _context.RiskProjects.Any(e => e.Id == id);
        }
    }
}
