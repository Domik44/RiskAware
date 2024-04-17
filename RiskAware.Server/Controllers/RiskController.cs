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

            if(riskProject.Scale == 3) // TODO -> use modulo
            {
                if(riskDto.Probability == 3)
                {
                    riskDto.Probability = 2;
                }
                else if(riskDto.Probability == 5)
                {
                    riskDto.Probability = 3;
                }

                if (riskDto.Impact == 3)
                {
                    riskDto.Impact = 2;
                }
                else if (riskDto.Impact == 5)
                {
                    riskDto.Impact = 3;
                }
            }

            var risk = new Risk
            {
                Created = DateTime.Now,
                UserId = activeUser.Id,
                RiskProjectId = riskProjectId,
                ProjectPhaseId = riskDto.ProjectPhaseId,
                RiskCategoryId = riskCategoryId
            };
            _context.Risks.Add(risk);
            _context.SaveChanges();

            var isApproved = activeUserRole == RoleType.RiskManager || activeUserRole == RoleType.ProjectManager;
            var riskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = activeUser.Id,
                Created = risk.Created,
                Title = riskDto.Title,
                Description = riskDto.Description,
                Probability = riskDto.Probability,
                Impact = riskDto.Impact,
                Threat = riskDto.Threat,
                Indicators = riskDto.Indicators,
                Prevention = riskDto.Prevention,
                Status = riskDto.Status,
                PreventionDone = riskDto.PreventionDone,
                RiskEventOccured = riskDto.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = DateTime.Now,
                End = riskDto.End,
                IsValid = true,
                IsApproved = isApproved
            };
            _context.RiskHistory.Add(riskHistory);
            _context.SaveChanges();

            // User call this, adds risk to db, then it would call fetch for getRisk and set active tab to display it
            // instead i can pass it directly here and leave one fetch? 
            // need to update RiskPageDto to include RiskDetailDto
            return Ok();
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
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }

            //risk.IsValid = false;
            //await _context.SaveChangesAsync();

            //return Ok();
            return null;
        }

        private bool RiskExists(int id)
        {
            return _context.Risks.Any(e => e.Id == id);
        }
    }
}
