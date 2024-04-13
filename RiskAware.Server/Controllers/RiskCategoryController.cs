using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RiskCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RiskCategory
        [HttpGet("/api/RiskProject/{riskId}/RiskCategories")]
        public async Task<ActionResult<IEnumerable<RiskCategory>>> GetRiskCategories(int riskId)
        {
            // TODO -> implement this method
            //return await _context.RiskCategories.ToListAsync();
            return null;
        }

        // GET: api/RiskCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskCategory>> GetRiskCategory(int id)
        {
            // TODO -> implement this method
            //var riskCategory = await _context.RiskCategories.FindAsync(id);

            //if (riskCategory == null)
            //{
            //    return NotFound();
            //}

            //return riskCategory;
            return null;
        }

        // POST: api/RiskCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RiskCategory>> CreateRiskCategory(RiskCategory riskCategory)
        {
            // TODO -> implement this method
            //_context.RiskCategories.Add(riskCategory);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetRiskCategory", new { id = riskCategory.Id }, riskCategory);
            return null;
        }

        // PUT: api/RiskCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRiskCategory(int id, RiskCategory riskCategory)
        {
            // TODO -> implement this method
            //if (id != riskCategory.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(riskCategory).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!RiskCategoryExists(id))
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

        // DELETE: api/RiskCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskCategory(int id)
        {
            // TODO -> implement this method
            //var riskCategory = await _context.RiskCategories.FindAsync(id);
            //if (riskCategory == null)
            //{
            //    return NotFound();
            //}

            //_context.RiskCategories.Remove(riskCategory);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return null;
        }

        private bool RiskCategoryExists(int id)
        {
            return _context.RiskCategories.Any(e => e.Id == id);
        }
    }
}
