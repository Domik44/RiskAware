using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
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

        // GET: api/ProjectPhases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectPhaseDetailDto>> GetProjectPhase(int id)
        {
            var projectPhase = await _projectPhaseQueries.GetProjectPhaseDetailAsync(id);

            if (projectPhase == null)
            {
                return NotFound();
            }

            return Ok(projectPhase);
        }

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

        // POST: api/ProjectPhases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/RiskProject/{riskProjectId}/CreateProjectPhase")]
        //public async Task<ActionResult<ProjectPhase>> CreateProjectPhase(int riskId, ProjectPhase projectPhase) // TODO -> depends if we want to just add new elem to the table or regenerate whole table in frontend
        public async Task<IActionResult> CreateProjectPhase(int riskProjectId, ProjectPhaseCreateDto projectPhaseDto) // TODO -> change to DTO
        {
            // first get activeUser
            //var activeUser = await _userManager.GetUserAsync(User);
            // then get project and check if it exists
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Risk project not found");
            }

            // then check if he has premmision to create phase -> projectManager, RiskManager??
            //var isProjectManager = await _projectRoleQueries.IsProjectManager(riskId, activeUser.Id);
            //var isRiskManager = await _projectRoleQueries.IsRiskManager(riskId, activeUser.Id);
            //if(!isProjectManager && !isRiskManager)
            if (projectPhaseDto.UserRoleType != RoleType.ProjectManager) // TODO -> could be attacked like this??
            {
                return Unauthorized();
            }

            var newProjectPhase = new ProjectPhase
            {
                Name = projectPhaseDto.Name,
                Start = projectPhaseDto.Start,
                End = projectPhaseDto.End,
                RiskProjectId = riskProjectId
            };

            _context.ProjectPhases.Add(newProjectPhase);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/ProjectPhases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectPhase(int id, ProjectPhase projectPhase)
        {
            // get active user
            // get project phase and check if it exists
            // check if user has permission to update phase -> projectManager, RiskManager??
            // update phase according to DTO

            // TODO -> implement this method
            //if (id != projectPhase.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(projectPhase).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProjectPhaseExists(id))
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

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/ProjectPhases/5
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

        private bool ProjectPhaseExists(int id)
        {
            return _context.ProjectPhases.Any(e => e.Id == id);
        }
    }
}
