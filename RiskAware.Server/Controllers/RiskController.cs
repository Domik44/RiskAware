using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    /// <summary>
    /// Controller for handling risk related requests.
    /// </summary>
    /// <author> Dominik Pop </author>
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

        /// <summary>
        /// Method for getting a specific risk detail.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns DTO containing info about risk. </returns>
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
        /// Method for getting all risks that belong to a specific risk project.
        /// </summary>
        /// <param name="id">Id of a risk project. </param>
        /// <returns> Collection of DTOs containing basic info about risk. </returns>
        [HttpGet("/api/RiskProject/{id}/Risks")]
        public async Task<ActionResult<IEnumerable<RiskDto>>> GetAllRiskProjectRisks(int id)
        {
            var risks = await _riskQueries.GetRiskProjectRisksAsync(id);

            return Ok(risks);
        }

        /// <summary>
        /// Method for getting all risks that belong to a specific project phase.
        /// </summary>
        /// <param name="id"> Id of phase. </param>
        /// <returns> Collection of DTOs containing basic info about risk. </returns>
        [HttpGet("/api/ProjectPhase/{id}/Risks")]
        public async Task<ActionResult<IEnumerable<RiskDto>>> GetAllProjectPhaseRisks(int id)
        {
            var risks = await _riskQueries.GetProjectPhaseRisksAsync(id);

            return Ok(risks);
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// Get filtered project's risks.
        /// </summary>
        /// <param name="dtParams"> Data table filtering parameters. </param>
        /// <returns> Filtered project's risks DTOs. </returns>
        [HttpPost("/api/RiskProject/{projectId}/Risks")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjectRisks(int projectId, [FromBody] DtParamsDto dtParams)
        {
            var query = _riskQueries.QueryProjectRisks(projectId, dtParams);
            int totalRowCount = await query.CountAsync();
            var risks = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResultDto<RiskDto>
            {
                Data = risks,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for creating a new risk.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="riskDto"> DTO containing info about new risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPost("/api/RiskProject/{riskProjectId}/AddRisk")]
        public async Task<IActionResult> CreateRisk(int riskProjectId, RiskCreateDto riskDto)
        {
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.RiskProjectId == riskProjectId && pp.Id == riskDto.ProjectPhaseId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return NotFound("Project phase not found");
            }

            var activeUser = await _userManager.GetUserAsync(User);;
            var isAllowedToAddRisk = await _projectRoleQueries.IsAllowedToAddEditRisk(riskProjectId, projectPhase.Id, activeUser.Id);
            if (!isAllowedToAddRisk)
            {
                return Unauthorized("User cannot add risk to this project!");
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

            riskDto.Probability = EditValue(riskDto.Probability, riskProject.Scale);
            riskDto.Impact = EditValue(riskDto.Impact, riskProject.Scale);
            var isApproved = _projectRoleQueries.IsAllowedToDeclineApproveRisk(riskProjectId, activeUser.Id).Result;

            await _riskQueries.AddRiskAsync(riskDto, activeUser.Id, riskProjectId, riskCategoryId, isApproved);

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        /// <summary>
        /// Method for updating a specific risk.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <param name="riskDto"> DTO containing new info about risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRisk(int id, RiskCreateDto riskDto)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound("Risk not found!");
            }

            var riskProject = await _context.RiskProjects.FindAsync(risk.RiskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.RiskProjectId == risk.RiskProjectId && pp.Id == risk.ProjectPhaseId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return NotFound("Project phase not found");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isAllowedToEdit = await _projectRoleQueries.IsAllowedToAddEditRisk(risk.RiskProjectId, projectPhase.Id, activeUser.Id);
            if (!isAllowedToEdit)
            {
                return Unauthorized("User in not allowed to edit this risk!");
            }

            var riskCategoryId = riskDto.RiskCategory.Id;
            if (riskCategoryId == -1) // new type was selected
            {
                var newRiskCategory = new RiskCategory
                {
                    Name = riskDto.RiskCategory.Name,
                    RiskProjectId = risk.RiskProjectId
                };
                _context.RiskCategories.Add(newRiskCategory);
                _context.SaveChanges();
                riskCategoryId = newRiskCategory.Id;
            }
            else // predefined type was selected
            {
                var riskCategory = await _context.RiskCategories.Where(rc => rc.RiskProjectId == riskProject.Id && rc.Id == riskCategoryId).FirstOrDefaultAsync();
                if (riskCategory == null)
                {
                    return NotFound("Risk category not found");
                }
            }

            riskDto.Probability = EditValue(riskDto.Probability, riskProject.Scale);
            riskDto.Impact = EditValue(riskDto.Impact, riskProject.Scale);

            await _riskQueries.EditRiskAsync(riskDto, activeUser.Id, risk, riskCategoryId, true);

            return Ok();
        }

        /// <summary>
        /// Method for restoring a specific risk.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound("Risk not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isAllowedToDelete = await _projectRoleQueries.IsAllowedToDeleteRestoreRisk(risk, activeUser.Id);
            if (!isAllowedToDelete)
            {
                return Unauthorized("You are not allowed to delete this risk!");
            }

            var riskHistory = await _context.RiskHistory.Where(h => h.RiskId == id).OrderByDescending(h => h.LastModif).FirstOrDefaultAsync();
            if (riskHistory == null)
            {
                return NotFound("Risk history not found!");
            }

            var newRiskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = activeUser.Id,
                Created = riskHistory.Created,
                Title = riskHistory.Title,
                Description = riskHistory.Description,
                Probability = riskHistory.Probability,
                Impact = riskHistory.Impact,
                Threat = riskHistory.Threat,
                Indicators = riskHistory.Indicators,
                Prevention = riskHistory.Prevention,
                Status = riskHistory.Status,
                PreventionDone = riskHistory.PreventionDone,
                RiskEventOccured = riskHistory.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = riskHistory.StatusLastModif,
                End = riskHistory.End,
                IsValid = true, // Undo soft delete
                IsApproved = riskHistory.IsApproved
            };

            _context.RiskHistory.Add(newRiskHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Method for approving a specific risk.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> ApproveRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound("Risk not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isAllowedToApprove = await _projectRoleQueries.IsAllowedToDeclineApproveRisk(id, activeUser.Id);
            if (!isAllowedToApprove)
            {
                return Unauthorized("User is not allowed to approve risk!");
            }

            var riskHistory = await _context.RiskHistory.Where(h => h.RiskId == id).OrderByDescending(h => h.LastModif).FirstOrDefaultAsync();
            if (riskHistory == null)
            {
                return NotFound("Risk history not found!");
            }

            var newRiskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = activeUser.Id,
                Created = riskHistory.Created,
                Title = riskHistory.Title,
                Description = riskHistory.Description,
                Probability = riskHistory.Probability,
                Impact = riskHistory.Impact,
                Threat = riskHistory.Threat,
                Indicators = riskHistory.Indicators,
                Prevention = riskHistory.Prevention,
                Status = riskHistory.Status,
                PreventionDone = riskHistory.PreventionDone,
                RiskEventOccured = riskHistory.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = riskHistory.StatusLastModif,
                End = riskHistory.End,
                IsValid = riskHistory.IsValid,
                IsApproved = true
            };
            _context.RiskHistory.Add(newRiskHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// Method for deleting a specific risk.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRisk(int id) // Delete is basically edit -> set isValid to false
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound("Risk not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isAllowedToDelete = await _projectRoleQueries.IsAllowedToDeleteRestoreRisk(risk, activeUser.Id);
            if (!isAllowedToDelete)
            {
                return Unauthorized("You are not allowed to delete this risk!");
            }

            var riskHistory = await _context.RiskHistory.Where(h => h.RiskId == id).OrderByDescending(h => h.LastModif).FirstOrDefaultAsync();
            if (riskHistory == null)
            {
                return NotFound("Risk history not found!");
            }

            var newRiskHistory = new RiskHistory
            {
                RiskId = risk.Id,
                UserId = activeUser.Id,
                Created = riskHistory.Created,
                Title = riskHistory.Title,
                Description = riskHistory.Description,
                Probability = riskHistory.Probability,
                Impact = riskHistory.Impact,
                Threat = riskHistory.Threat,
                Indicators = riskHistory.Indicators,
                Prevention = riskHistory.Prevention,
                Status = riskHistory.Status,
                PreventionDone = riskHistory.PreventionDone,
                RiskEventOccured = riskHistory.RiskEventOccured,
                LastModif = DateTime.Now,
                StatusLastModif = riskHistory.StatusLastModif,
                End = riskHistory.End,
                IsValid = false, // Soft delete
                IsApproved = riskHistory.IsApproved
            };

            _context.RiskHistory.Add(newRiskHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Method for rejecting a specific risk.
        /// </summary>
        /// <param name="id"> Id of risk. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpDelete("{id}/Reject")]
        public async Task<IActionResult> RejectRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound("Risk not found!");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            var isAllowedToDecline = await _projectRoleQueries.IsAllowedToDeclineApproveRisk(id, activeUser.Id);
            if (!isAllowedToDecline)
            {
                return Unauthorized("User is not allowed to approve risk!");
            }

            _context.Risks.Remove(risk);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PRIVATE METHODS //////////////////
        
        /// <summary>
        /// Method for editing the value of probability or impact based on the scale of the project.
        /// </summary>
        /// <param name="value"> Value taken from frontend. </param>
        /// <param name="scale"> Scale of project. </param>
        /// <returns></returns>
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
