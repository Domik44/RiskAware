using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RiskQueries _riskQueries;

        public RiskController(AppDbContext context, RiskQueries riskQueries)
        {
            _context = context;
            _riskQueries = riskQueries;
        }

        ////////////////// GET METHODS //////////////////

        // GET: api/Risk/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Risk>> GetRisk(int id)
        {
            // TODO -> implement this method
            //var risk = await _context.Risks.FindAsync(id);

            //if (risk == null)
            //{
            //    return NotFound();
            //}

            //return risk;
            return null;
        }

        /// <summary>
        /// This controller method returns all risks for a specific project.
        /// </summary>
        /// 
        /// <param name="id">Id of a RiskProject</param>
        /// <returns> Returns DTO for risk project risks tab. </returns>
        [HttpGet("/api/RiskProject/{id}/Risks")]
        public async Task<ActionResult<IEnumerable<RiskDto>>> GetAllRiskProjectRisks(int id)
        {
            var risks = await _riskQueries.GetRiskProjectRisksAsync(id);

            return Ok(risks);
        }

        [HttpGet("/api/ProjectPhase/{id}/Risks")]
        public async Task<ActionResult<IEnumerable<RiskDto>>> GetAllProjectPhaseRisks(int id)
        {
            // TODO -> implement this method
            //var risks = await _riskQueries.GetRiskProjectRisksAsync(id);

            //return Ok(risks);
            return null;
        }

        ////////////////// POST METHODS //////////////////

        // POST: api/Risk
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Risk>> CreateRisk(Risk risk)
        {
            // TODO -> implement this method
            //_context.Risks.Add(risk);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetRisk", new { id = risk.Id }, risk);
            return null;
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/Risk/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRisk(int id, Risk risk)
        {
            // TODO -> implement this method
            //if (id != risk.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(risk).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!RiskExists(id))
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

        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreRisk(int id)
        {
            // TODO -> implement this method
            return null;
        }

        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> ApproveRisk(int id)
        {
            // TODO -> implement this method
            return null;
        }

        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectRisk(int id)
        {
            // TODO -> implement this method
            return null;
        }

        ////////////////// DELETE METHODS //////////////////
        
        // DELETE: api/Risk/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRisk(int id)
        {
            // TODO -> implement this method
            //var risk = await _context.Risks.FindAsync(id);
            //if (risk == null)
            //{
            //    return NotFound();
            //}

            //_context.Risks.Remove(risk);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return null;
        }

        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.Id == id);
        }
    }
}
