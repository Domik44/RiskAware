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
    /// <summary>
    /// Controller for managing risk categories related requests.
    /// </summary>
    /// <author>Dominik Pop</author>
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

        /// <summary>
        /// Get all risk categories for a specific risk project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <returns> Returns collection of DTOs containing basic info about risk category. </returns>
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

        /// <summary>
        /// Method for getting info about risk category.
        /// </summary>
        /// <param name="id"> Id of risk category. </param>
        /// <returns> Returns DTO containing basic info about risk category. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskCategoryDto>> GetRiskCategory(int id)
        {
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

        /// <summary>
        /// Method for creating new risk category.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="riskCategoryDto"> DTO containing basic info about risk category. </param>
        /// <returns> Returns id of new risk category. </returns>
        [HttpPost("{riskprojectId}")]
        public async Task<ActionResult<int>> CreateRiskCategory(int riskProjectId, RiskCategoryDto riskCategoryDto)
        {
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var hasBasicEditPermissions = await _projectRoleQueries.HasBasicEditPermissions(riskProjectId, activeUser.Id);
            if (!hasBasicEditPermissions)
            {
                return Unauthorized();
            }

            var newRiskCategory = new RiskCategory
            {
                Name = riskCategoryDto.Name,
                RiskProjectId = riskProjectId
            };

            _context.RiskCategories.Add(newRiskCategory);
            await _context.SaveChangesAsync();

            return newRiskCategory.Id;
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateRiskCategory(int id, RiskCategory riskCategory)
        //{
        //    // TODO -> implement
        //    return null;
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRiskCategory(int id)
        //{
        //    // TODO -> implement
        //    return null;
        //}
    }
}
