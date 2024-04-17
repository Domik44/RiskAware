using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Authorize]
    public class RiskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RiskQueries _riskQueries;
        private readonly ProjectRoleQueries _projectRoleQueries;

        public RiskController(AppDbContext context, RiskQueries riskQueries, UserManager<User> userManager, ProjectRoleQueries projectRoleQueries)
        {
            _context = context;
            _riskQueries = riskQueries;
            _userManager = userManager;
            _projectRoleQueries = projectRoleQueries;
        }

        ////////////////// GET METHODS //////////////////

        // GET: api/Risk/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskDetailDto>> GetRisk(int id)
        {
            var risk = await _riskQueries.GetRiskDetailAsync(id);

            if (risk == null)
            {
                return NotFound();
            }

            return Ok(risk);
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
            var risks = await _riskQueries.GetProjectPhaseRisksAsync(id);

            return Ok(risks);
        }

        ////////////////// POST METHODS //////////////////

        // POST: api/Risk
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/RiskProject/{riskProjectId}/AddRisk")]
        public async Task<IActionResult> CreateRisk(int riskProjectId, RiskCreateDto riskDto) // TODO ->return risk or just action result?
        {
            var activeUser = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId); // TODO -> mby do better like exists or something
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            //var activeUserRole = await _projectRoleQueries.GetUsersRoleOnRiskProjectAsync(riskProjectId, activeUser.Id);
            var activeUserRole = riskDto.UserRoleType;
            if(activeUserRole != RoleType.RiskManager && activeUserRole != RoleType.ProjectManager && activeUserRole != RoleType.TeamMember)
            {
                return Unauthorized();
            }

            var riskCategoryId = riskDto.RiskCategory.Id;
            if(riskCategoryId == -1) // new type was selected
            {
                var newRiskCategory = new RiskCategory
                {
                    Name = riskDto.RiskCategory.Name,
                    RiskProjectId = riskProjectId
                };
                _context.RiskCategories.Add(newRiskCategory);
                _context.SaveChanges();
                riskCategoryId = newRiskCategory.Id;
            }
            else // predefined type was selected
            {
                var riskCategory = await _context.RiskCategories.Where(rc => rc.RiskProjectId == riskProjectId && rc.Id == riskCategoryId).FirstOrDefaultAsync();
                if(riskCategory == null)
                {
                    return NotFound("Risk category not found");
                }
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.RiskProjectId == riskProjectId && pp.Id == riskDto.ProjectPhaseId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return NotFound("Project phase not found");
            }

            riskDto.Probability = EditValue(riskDto.Probability, riskProject.Scale);
            riskDto.Impact = EditValue(riskDto.Impact, riskProject.Scale);
            var isApproved = activeUserRole == RoleType.RiskManager || activeUserRole == RoleType.ProjectManager;

            await _riskQueries.AddRiskAsync(riskDto, activeUser.Id, riskProjectId, riskCategoryId, isApproved);

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/Risk/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRisk(int id, RiskCreateDto riskDto)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            // TODO -> check if user is allowed to edit this risk

            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }
            risk.RiskCategoryId = riskDto.RiskCategory.Id; // TODO -> check
            risk.ProjectPhaseId = riskDto.ProjectPhaseId; /// TODO -> check

            var scale = risk.RiskProject.Scale;
            // TODO -> dates -> will have to get last history and check if something changed -> then set according dates
            var newRiskHistory = new RiskHistory
            {
                Created = DateTime.Now,
                UserId = activeUser.Id,
                RiskId = risk.Id,
                Title = riskDto.Title,
                Description = riskDto.Description,
                Probability = EditValue(riskDto.Probability, scale),
                Impact = EditValue(riskDto.Impact, scale),
                Status = riskDto.Status,
                RiskEventOccured = riskDto.RiskEventOccured,
                //IsApproved = riskDto.IsApproved // TODO 
            };
            // TODO -> implement this method
            return null;
        }

        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreRisk(int id)
        {
            // TODO -> implement this method
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            //risk.IsValid = true; // TODO -> rn is in RiskHistory, maybe I will move it so I can do it like this
            //await _context.SaveChangesAsync();

            return null;
            //return Ok();
        }

        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> ApproveRisk(int id)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            // TODO -> implement this method
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            var riskHistory = await _context.RiskHistory.Where(rh => rh.RiskId == id).OrderByDescending(rh => rh.Created).FirstOrDefaultAsync();
            var newRiskHistory = new RiskHistory // TODO -> copy from old and change only isApproved
            {
                Created = DateTime.Now,
                UserId = riskHistory.UserId,
                RiskId = risk.Id,
                Title = riskHistory.Title,
                Description = riskHistory.Description,
                Probability = riskHistory.Probability,
                Impact = riskHistory.Impact,
                Status = riskHistory.Status,
                RiskEventOccured = riskHistory.RiskEventOccured,
                IsApproved = true
            };

            return null;
            return Ok();
        }

        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectRisk(int id)
        {
            // TODO -> implement this method
            var activeUser = await _userManager.GetUserAsync(User);
            // TODO -> check user premissions
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            var riskHistory = await _context.RiskHistory.Where(rh => rh.RiskId == id).OrderByDescending(rh => rh.Created).FirstOrDefaultAsync();
            // TODO -> should delete risk
            return null;
            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/Risk/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRisk(int id)
        {
            // TODO -> implement this method
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            var riskHistory = await _context.RiskHistory.Where(rh => rh.RiskId == id).OrderByDescending(rh => rh.Created).FirstOrDefaultAsync();
            // TODO -> should create new history and soft delete
            //risk.IsValid = false;
            //await _context.SaveChangesAsync();

            return null;
            return Ok();
        }

        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.Id == id);
        }

        private int EditValue(int value, int scale)
        {
            switch (scale)
            {
                case 3 when value == 3:
                    return 2;
                case 3 when value == 5:
                    return 3;
                default:
                    return value;
            }
        }
    }
}
