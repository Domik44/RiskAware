using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs;
using RiskAware.Server.Models;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskProjectController : ControllerBase // TODO -> switch na DTO!!
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public RiskProjectController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// This controller method return all projects that are stored in database.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        /// url : /api/RiskProjects
        [Route("/api/RiskProjects")]
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
        /// url : /api/RiskProject/AdminRiskProjects
        [HttpGet("AdminRiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetAdminRiskProjects()
        {
            var user = await _userManager.GetUserAsync(User);

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
        /// url : /api/RiskProject/UserRiskProjects
        [HttpGet("UserRiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetUserRiskProjects()
        {
            var user = await _userManager.GetUserAsync(User);

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

            return riskProjects;
            //return Ok(riskProjects);
        }

        /// <summary>
        /// This controller method returns a detail of a project with specific id.
        /// </summary>
        /// 
        /// <param name="id"> Id of a project. </param>
        /// <returns> Returns DTO used for risk project detail. </returns>
        /// url: api/RiskProjects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskProjectPageDto>> GetRiskProject(int id)
        {
            // TODO -> define queries so they dont repeat in here
            var riskProject = await _context.RiskProjects
                .AsNoTracking()
                .Where(pr => pr.Id == id)
                .Include(pr => pr.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();

            if (riskProject == null)
            {
                return NotFound();
            }

            var projectRoles = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == id)
                .Include(pr => pr.User)
                .Select(pr => new ProjectRoleDto
                {
                    Id = pr.Id,
                    RoleName = pr.RoleType.ToString(),
                    IsReqApproved = pr.IsReqApproved,
                    User = new UserDto
                    {
                        Email = pr.User.Email,
                        FullName = pr.User.FirstName + " " + pr.User.LastName
                    }
                })
                .ToListAsync();

            var risks = _context.Risks
                .AsNoTracking()
                .Where(r => r.RiskProjectId == id)
                .Include(r => r.RiskCategory)
                .Select(r => new
                {
                    Risk = r,
                    LatestRiskHistory = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .FirstOrDefault()
                })
                .AsEnumerable() // Switch to client-side execution for the DTO projection ??
                .Select(r => new RiskDto
                {
                    Id = r.Risk.Id,
                    Title = r.LatestRiskHistory?.Title,
                    CategoryName = r.Risk.RiskCategory.Name,
                    Severity = r.LatestRiskHistory?.Probability * r.LatestRiskHistory?.Impact ?? 0,
                    State = r.LatestRiskHistory?.Status
                })
                .ToList();

            var projectPhases = await _context.ProjectPhases
                .AsNoTracking()
                .Where(pp => pp.RiskProjectId == id)
                .Include(pp => pp.Risks)
                .Select(pp => new ProjectPhaseDto // TODO -> mby put this in DTO constructor
                {
                    Id = pp.Id,
                    Order = pp.Order,
                    Name = pp.Name,
                    Start = pp.Start,
                    End = pp.End,
                    Risks = pp.Risks.Select(r => new RiskUnderPhaseDto
                    {
                        Id = r.Id,
                        Title = r.RiskHistory.OrderByDescending(h => h.Created).FirstOrDefault().Title, // TODO -> mby add Title to Risk entity, in this case the redudancy will be minimal and it will be easier to get the title
                    })

                })
                .ToListAsync();

            var riskProjectPage = new RiskProjectPageDto
            {
                Detail = new RiskProjectDetailDto(riskProject),
                Members = projectRoles,
                Risks = risks,
                Phases = projectPhases
            };

            return riskProjectPage;
        }

        [HttpGet("{id}/Detail")]
        public async Task<ActionResult<RiskProjectDetailDto>> GetRiskProjectDetail(int id)
        {
            var riskProject = await _context.RiskProjects
                .AsNoTracking()
                .Where(pr => pr.Id == id)
                .Include(pr => pr.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();

            if (riskProject == null)
            {
                return NotFound();
            }

            var riskProjectDetail = new RiskProjectDetailDto(riskProject);

            return riskProjectDetail;
        }

        [HttpGet("{id}/Phases")]
        public async Task<ActionResult<IEnumerable<ProjectPhaseDto>>> GetRiskProjectPhases(int id)
        {
            var projectPhases = await _context.ProjectPhases
                .AsNoTracking()
                .Where(pp => pp.RiskProjectId == id)
                .Include(pp => pp.Risks)
                .OrderBy(pp => pp.Order)
                .Select(pp => new ProjectPhaseDto // TODO -> mby put this in DTO constructor
                {
                    Id = pp.Id,
                    Order = pp.Order,
                    Name = pp.Name,
                    Start = pp.Start,
                    End = pp.End,
                    Risks = pp.Risks.Select(r => new RiskUnderPhaseDto
                    {
                        Id = r.Id,
                        Title = r.RiskHistory.OrderByDescending(h => h.Created).FirstOrDefault().Title, // TODO -> mby add Title to Risk entity, in this case the redudancy will be minimal and it will be easier to get the title
                    })

                })
                .ToListAsync();

            return projectPhases;
        }

        [HttpGet("{id}/Risks")]
        public async Task<ActionResult<IEnumerable<RiskDto>>> GetRiskProjectRisks(int id)
        {
            var risks =  _context.Risks // TODO -> do it better
                .AsNoTracking()
                .Where(r => r.RiskProjectId == id)
                .Include(r => r.RiskCategory)
                .Select(r => new
                {
                    Risk = r,
                    LatestRiskHistory = r.RiskHistory
                        .OrderByDescending(h => h.Created)
                        .FirstOrDefault()
                })
                .AsEnumerable() // Switch to client-side execution for the DTO projection ??
                .Select(r => new RiskDto // TODO -> mby put this in DTO constructor
                {
                    Id = r.Risk.Id,
                    Title = r.LatestRiskHistory?.Title,
                    CategoryName = r.Risk.RiskCategory.Name,
                    Severity = r.LatestRiskHistory?.Probability * r.LatestRiskHistory?.Impact ?? 0,
                    State = r.LatestRiskHistory?.Status
                })
                .ToList();

            return risks;
        }

        [HttpGet("{id}/Members")]
        public async Task<ActionResult<IEnumerable<ProjectRoleDto>>> GetRiskProjectMembers(int id)
        {
            // i need to get all project roles for a certain risk project and then get all users from that project roles and then get phases for each project role
            // TODO -> add navigation property to ProjectRole for ProjectPhase so I can include it as well

            var projectRoles = await _context.ProjectRoles
                .AsNoTracking()
                .Where(pr => pr.RiskProjectId == id)
                .Include(pr => pr.User)
                .Select(pr => new ProjectRoleDto // TODO -> mby put this in DTO constructor
                {
                    Id = pr.Id,
                    RoleName = pr.RoleType.ToString(),
                    IsReqApproved = pr.IsReqApproved,
                    User = new UserDto
                    {
                        Email = pr.User.Email,
                        FullName = pr.User.FirstName + " " + pr.User.LastName
                    }
                })
                .ToListAsync();

            return projectRoles;
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
