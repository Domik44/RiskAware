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
    public class RiskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RiskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Risk
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Risk>>> GetRisks()
        {
            return await _context.Risks.ToListAsync();
        }

        // GET: api/Risk/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Risk>> GetRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);

            if (risk == null)
            {
                return NotFound();
            }

            return risk;
        }

        // PUT: api/Risk/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRisk(int id, Risk risk)
        {
            if (id != risk.Id)
            {
                return BadRequest();
            }

            _context.Entry(risk).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiskExists(id))
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

        // POST: api/Risk
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Risk>> PostRisk(Risk risk)
        {
            _context.Risks.Add(risk);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRisk", new { id = risk.Id }, risk);
        }

        // DELETE: api/Risk/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            _context.Risks.Remove(risk);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.Id == id);
        }
    }
}
