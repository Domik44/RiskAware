using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPhaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ProjectPhaseQueries _projectPhaseQueries;

        public ProjectPhaseController(AppDbContext context, ProjectPhaseQueries projectPhaseQueries)
        {
            _context = context;
            _projectPhaseQueries = projectPhaseQueries;
        }

        ////////////////// GET METHODS //////////////////
        
        // GET: api/ProjectPhases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectPhase>> GetProjectPhase(int id)
        {
            // TODO -> implement this method
            //var projectPhase = await _context.ProjectPhases.FindAsync(id);

            //if (projectPhase == null)
            //{
            //    return NotFound();
            //}

            //return projectPhase;
            return null;
        }

        /// <summary>
        /// This controller method returns all phases for a specific project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns>Returns DTO for risk project phases tab.</returns>
        [HttpGet("/api/RiskProject/{id}/Phases")] // TODO -> mby specify route so it makes more sense -> like api/RiskProject/5/Pahses ??
        public async Task<ActionResult<IEnumerable<ProjectPhaseDto>>> GetAllRiskProjectPhases(int id)
        {
            var projectPhases = await _projectPhaseQueries.GetRiskProjectPhasesAsync(id);

            return Ok(projectPhases);
        }

        ////////////////// POST METHODS //////////////////
        
        // POST: api/ProjectPhases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectPhase>> CreateProjectPhase(ProjectPhase projectPhase)
        {
            // TODO -> implement this method
            //_context.ProjectPhases.Add(projectPhase);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProjectPhase", new { id = projectPhase.Id }, projectPhase);
            return null;
        }

        ////////////////// PUT METHODS //////////////////
        
        // PUT: api/ProjectPhases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectPhase(int id, ProjectPhase projectPhase)
        {
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
