using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Authorize]
    public class RiskCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ProjectRoleQueries _projectRoleQueries;

        public RiskCategoryController(AppDbContext context, UserManager<User> userManager, ProjectRoleQueries projectRoleQueries)
        {
            _context = context;
            _userManager = userManager;
            _projectRoleQueries = projectRoleQueries;
        }

        // GET: api/RiskCategory
        [HttpGet("/api/RiskProject/{riskProjectId}/RiskCategories")]
        public async Task<ActionResult<IEnumerable<RiskCategoryDto>>> GetRiskCategories(int riskProjectId)
        {
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound();
            }

            var riskCategories = await _context.RiskCategories
                .AsNoTracking()
                .Where(rc => rc.RiskProjectId == riskProjectId)
                .Select(rc => new RiskCategoryDto
                {
                    Id = rc.Id,
                    Name = rc.Name,
                })
                .ToListAsync();

            return Ok(riskCategories);
        }

        // GET: api/RiskCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskCategoryDto>> GetRiskCategory(int id)
        {
            // TODO -> implement this method
            var riskCategory = await _context.RiskCategories.FindAsync(id);

            if (riskCategory == null)
            {
                return NotFound();
            }

            var riskCategoryDto = new RiskCategoryDto
            {
                Id = riskCategory.Id,
                Name = riskCategory.Name
            };

            return riskCategoryDto;
        }

        // POST: api/RiskCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> CreateRiskCategory(int riskId, RiskCategoryDto riskCategoryDto)
        {
            // risk category will be created when a risk is created
            // user will either select an existing risk category or create a new one
            // if user creates a new one, this method will be called

            // in fronted we will need to check if risk category is being created or selected
            // if it is being created, we will call this method and retrieve the risk category
            // then new risk can be created by calling proper controller method

            // risk category is assgined to a risk project so we need to know the risk project id
            // then we get the risk project and check if it exists
            var riskProject = await _context.RiskProjects.FindAsync(riskId);
            if (riskProject == null)
            {
                return NotFound();
            }

            // then we need to check if the user has permission to create a risk category for that risk project -> project manager or risk manager or team member
            var activeUser = await _userManager.GetUserAsync(User);
            if (!_projectRoleQueries.HasBasicEditPermissions(riskId, activeUser.Id).Result)
            {
                return Unauthorized();
            }

            // then we create the risk category and assign it to the risk project
            var newRiskCategory = new RiskCategory
            {
                Name = riskCategoryDto.Name,
                RiskProjectId = riskId
            };

            _context.RiskCategories.Add(newRiskCategory);
            await _context.SaveChangesAsync();

            return newRiskCategory.Id;
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
            // logic here will be harder as we need to check if there are any risks that are using this risk category
            // if there are, we need to reassign them to another risk category or delete them
            // we also need to check if user has permission to delete the risk category

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
    }
}
