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
    public class RiskCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RiskCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RiskCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RiskCategory>>> GetRiskCategories()
        {
            return await _context.RiskCategories.ToListAsync();
        }

        // GET: api/RiskCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskCategory>> GetRiskCategory(Guid id)
        {
            var riskCategory = await _context.RiskCategories.FindAsync(id);

            if (riskCategory == null)
            {
                return NotFound();
            }

            return riskCategory;
        }

        // PUT: api/RiskCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRiskCategory(Guid id, RiskCategory riskCategory)
        {
            if (id != riskCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(riskCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiskCategoryExists(id))
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

        // POST: api/RiskCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RiskCategory>> PostRiskCategory(RiskCategory riskCategory)
        {
            _context.RiskCategories.Add(riskCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRiskCategory", new { id = riskCategory.Id }, riskCategory);
        }

        // DELETE: api/RiskCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskCategory(Guid id)
        {
            var riskCategory = await _context.RiskCategories.FindAsync(id);
            if (riskCategory == null)
            {
                return NotFound();
            }

            _context.RiskCategories.Remove(riskCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RiskCategoryExists(Guid id)
        {
            return _context.RiskCategories.Any(e => e.Id == id);
        }
    }
}
