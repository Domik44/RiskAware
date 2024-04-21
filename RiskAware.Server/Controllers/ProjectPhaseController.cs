using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectPhaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ProjectPhaseQueries _projectPhaseQueries;
        private readonly ProjectRoleQueries _projectRoleQueries;

        public ProjectPhaseController(AppDbContext context, UserManager<User> userManager, ProjectPhaseQueries projectPhaseQueries, ProjectRoleQueries projectRoleQueries)
        {
            _context = context;
            _userManager = userManager;
            _projectPhaseQueries = projectPhaseQueries;
            _projectRoleQueries = projectRoleQueries;
        }

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// Method for getting all project phases for a specific project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns> Returns DTO for risk project phases tab. </returns>
        [HttpGet("/api/RiskProject/{id}/Phases")]
        public async Task<ActionResult<IEnumerable<ProjectPhaseDto>>> GetAllRiskProjectPhases(int id)
        {
            var projectPhases = await _projectPhaseQueries.GetRiskProjectPhasesAsync(id);

            return Ok(projectPhases);
        }

        /// <summary>
        /// Method for getting project phase based on id.
        /// </summary>
        /// <param name="id"> Id of phase. </param>
        /// <returns> DTO for project phase edit. </returns>
        /// url: api/ProjectPhase/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectPhaseCreateDto>> GetProjectPhase(int id)
        {
            var projectPhase = await _context.ProjectPhases.FindAsync(id);

            if (projectPhase == null)
            {
                return NotFound();
            }

            return Ok(new ProjectPhaseCreateDto
            {
                Name = projectPhase.Name,
                Start = projectPhase.Start,
                End = projectPhase.End,
                RiskProjectId = projectPhase.RiskProjectId
            });
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// Get filtered project phases
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="dtParams">Data table filtering parameters</param>
        /// <returns>Filtered project phases DTOs</returns>
        [HttpPost("/api/RiskProject/{projectId}/Phases")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjectPhases(int projectId, [FromBody] DtParamsDto dtParams)
        {
            var query = _projectPhaseQueries.QueryProjectPhases(projectId, dtParams);
            int totalRowCount = await query.CountAsync();
            var phases = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResultDto<ProjectPhaseDto>
            {
                Data = phases,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for creating project phase based on user input.
        /// </summary>
        /// <param name="projectPhaseDto"> DTO containing info about created phase. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        [HttpPost("/api/RiskProject/CreateProjectPhase")]
        public async Task<IActionResult> CreateProjectPhase(ProjectPhaseCreateDto projectPhaseDto)
        {
            var riskProjectId = projectPhaseDto.RiskProjectId;
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            var user = await _userManager.GetUserAsync(User);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(riskProjectId, user.Id);
            if (!isProjectManager)
            {
                return Unauthorized("User is not project manager!");
            }

            var order = _context.ProjectPhases.Where(pp => pp.RiskProjectId == riskProjectId).Count() + 1;
            var newProjectPhase = new ProjectPhase
            {
                Name = projectPhaseDto.Name,
                Start = projectPhaseDto.Start,
                End = projectPhaseDto.End,
                RiskProjectId = riskProjectId,
                Order = order
            };

            _context.ProjectPhases.Add(newProjectPhase);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////
        /// <summary>
        /// Method for updating project phase based on user input.
        /// </summary>
        /// <param name="phaseId"> Id of phase.</param>
        /// <param name="projectPhaseDto">DTO containing info about phase.</param>
        /// <returns>Returns operation result.</returns>
        ///url: api/ProjectPhase/{phaseId}
        [HttpPut("{phaseId}")]
        public async Task<IActionResult> UpdateProjectPhase(int phaseId, ProjectPhaseCreateDto projectPhaseDto)
        {
            var riskProjectId = projectPhaseDto.RiskProjectId;
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            var user = await _userManager.GetUserAsync(User);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(riskProjectId, user.Id);
            if (!isProjectManager)
            {
                return Unauthorized("User is not project manager!");
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.Id == phaseId && pp.RiskProjectId == riskProjectId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return BadRequest("Project phase not found");
            }

            projectPhase.Name = projectPhaseDto.Name;
            projectPhase.Start = projectPhaseDto.Start;
            projectPhase.End = projectPhaseDto.End;
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// Method for deleting project phase.
        /// </summary>
        /// <param name="id"> Id of phase. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/ProjectPhase/{id}
        [HttpDelete("{projectPhaseId}")]
        public async Task<IActionResult> DeleteProjectPhase(int projectPhaseId)
        {
            var phase = await _context.ProjectPhases.FindAsync(projectPhaseId);
            if (phase == null)
            {
                return NotFound("Project was not found!");
            }

            var user = await _userManager.GetUserAsync(User);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(phase.RiskProjectId, user.Id);
            if (!isProjectManager)
            {
                return Unauthorized("User is not project manager!");
            }

            var risks = await _context.Risks.Where(r => r.ProjectPhaseId == projectPhaseId).ToListAsync();
            if (risks.Count > 0)
            {
                return BadRequest("Phase is in use!");
            }

            _context.ProjectPhases.Remove(phase);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
