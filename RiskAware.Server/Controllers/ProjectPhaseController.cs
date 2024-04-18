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
        /// This controller method returns all phases for a specific project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns>Returns DTO for risk project phases tab.</returns>
        [HttpGet("/api/RiskProject/{id}/Phases")]
        public async Task<ActionResult<IEnumerable<ProjectPhaseDto>>> GetAllRiskProjectPhases(int id)
        {
            var projectPhases = await _projectPhaseQueries.GetRiskProjectPhasesAsync(id);

            return Ok(projectPhases);
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
        /// 
        /// </summary>
        /// <param name="projectPhaseDto"></param>
        /// <returns></returns>
        [HttpPost("/api/RiskProject/CreateProjectPhase")]
        public async Task<IActionResult> CreateProjectPhase(ProjectPhaseCreateDto projectPhaseDto)
        {
            var riskProjectId = projectPhaseDto.RiskProjectId;
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            if (projectPhaseDto.UserRoleType != RoleType.ProjectManager) // TODO -> could be attacked like this??
            {
                return Unauthorized();
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
        //url: api/ProjectPhase/{phaseId}
        [HttpPut("{phaseId}")]
        public async Task<IActionResult> UpdateProjectPhase(int phaseId, ProjectPhaseCreateDto projectPhaseDto)
        {
            var riskProjectId = projectPhaseDto.RiskProjectId;
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            if (projectPhaseDto.UserRoleType != RoleType.ProjectManager) // TODO -> could be attacked like this??
            {
                return Unauthorized();
            }

            var projectPhase = await _context.ProjectPhases.Where(pp => pp.Id == phaseId && pp.RiskProjectId == riskProjectId).FirstOrDefaultAsync();
            if (projectPhase == null)
            {
                return BadRequest("Project phase not found");
            }

            projectPhase.Name = projectPhaseDto.Name;
            projectPhase.Start = projectPhaseDto.Start;
            projectPhase.End = projectPhaseDto.End;
            _context.SaveChanges();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectPhase(int id)
        {
            // get active user
            // get project phase and check if it exists
            // check if user has permission to delete phase -> projectManager, RiskManager??
            // then check if phase is not in use -> if it is not in use delete it
            // else return error message -> phase is in use
            // TODO -> think about soft delete

            // TODO -> implement this method
            //var projectPhase = await _context.ProjectPhases.FindAsync(id);
            //if (projectPhase == null)
            //{
            //    return NotFound();
            //}

            //_context.ProjectPhases.Remove(projectPhase);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return null;
        }
    }
}
