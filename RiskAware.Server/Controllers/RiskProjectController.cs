using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskProjectController : ControllerBase // TODO -> switch na DTO!!
    {
        private readonly AppDbContext _context;

        public RiskProjectController(AppDbContext context)
        {
            _context = context;
        }

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// This controller method return all projects that are stored in database.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetRiskProjects()
        {
            return await _context.RiskProjects
                .Select(u =>
                    new RiskProjectDto
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Start = u.Start,
                        End = u.End,
                        NumOfMembers = u.ProjectRoles.Count,
                        ProjectManagerName = u.ProjectRoles
                            .Where(pr => pr.RoleType == RoleType.ProjectManager)
                            .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                            .FirstOrDefault()

                    }
                )
                .ToListAsync();
        }

        /// <summary>
        /// This controller method returns all projects that were created by admin only if logged user is admin.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        [HttpGet("AdminRiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetAdminRiskProjects()
        {
            //var user = User.Identity; // TODO -> switch na tohle
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == "d6f46418-2c21-43f8-b167-162fb5e3a999");

            if (user == null)
            {
                return NoContent();
            }
            else if (!user.SystemRole.IsAdministrator)
            {
                return Unauthorized();
            }

            var query = from riskProject in _context.RiskProjects
                        where riskProject.UserId == user.Id
                        select new RiskProjectDto
                        {
                            Id = riskProject.Id,
                            Title = riskProject.Title,
                            Start = riskProject.Start,
                            End = riskProject.End,
                            NumOfMembers = riskProject.ProjectRoles.Count,
                            ProjectManagerName = riskProject.ProjectRoles
                                .Where(pr => pr.RoleType == RoleType.ProjectManager)
                                .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                                .FirstOrDefault()
                        };

            var riskProjects = query.ToList();

            return riskProjects;
        }

        /// <summary>
        /// This controller method serves for getting all projects where user has a role.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        [HttpGet("UserRiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetUserRiskProjects()
        {
            //var user = User.Identity; // TODO -> switch na tohle
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == "5862be25-6467-450e-81fa-1cac9578650b");

            if (user == null)
            {
                //TODO -> user is not logged in
                return NoContent();
            }


            var query = from projectRole in _context.ProjectRoles
                        where projectRole.UserId == user.Id
                        join riskProject in _context.RiskProjects on projectRole.RiskProjectId equals riskProject.Id
                        select new RiskProjectDto
                        {
                            Id = riskProject.Id,
                            Title = riskProject.Title,
                            Start = riskProject.Start,
                            End = riskProject.End,
                            NumOfMembers = riskProject.ProjectRoles.Count,
                            ProjectManagerName = riskProject.ProjectRoles
                                .Where(pr => pr.RoleType == RoleType.ProjectManager)
                                .Select(pr => pr.User.FirstName + " " + pr.User.LastName)
                                .FirstOrDefault()
                        };
            var riskProjects = query.ToList();

            return Ok(riskProjects);
        }

        /// <summary>
        /// This controller method returns a detail of a project with specific id.
        /// </summary>
        /// 
        /// <param name="id"> Id of a project. </param>
        /// <returns> Returns DTO used for risk project detail. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskProject>> GetRiskProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);

            if (riskProject == null)
            {
                return NotFound();
            }

            return riskProject;
        }

        ////////////////// POST METHODS //////////////////

        // POST: api/RiskProjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RiskProject>> PostRiskProject(RiskProject riskProject)
        {
            //User user = (User)User.Identity;
            //if (user.SystemRole.IsAdministrator)
            //{
            //    _context.RiskProjects.Add(riskProject);
            //    await _context.SaveChangesAsync();

            //}
            //else
            //{
            //    //TODO -> not authorized
            //}

            _context.RiskProjects.Add(riskProject);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRiskProject", new { id = riskProject.Id }, riskProject);
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/RiskProjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRiskProject(int id, RiskProject riskProject)
        {
            if (id != riskProject.Id)
            {
                return BadRequest();
            }

            _context.Entry(riskProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiskProjectExists(id))
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

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/RiskProjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }

            _context.RiskProjects.Remove(riskProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RiskProjectExists(int id)
        {
            return _context.RiskProjects.Any(e => e.Id == id);
        }
    }
}
