using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;
using RiskAware.Server.ViewModels;
using System.Linq.Dynamic.Core;


namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskProjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RiskProjectQueries _riskProjectQueries;

        public RiskProjectController(AppDbContext context, UserManager<User> userManager, RiskProjectQueries riskProjectQueries)
        {
            _context = context;
            _userManager = userManager;
            _riskProjectQueries = riskProjectQueries;
        }

        ////////////////// GET METHODS //////////////////
        // TODO: delete if unused
        /// <summary>
        /// This controller method return all projects that are stored in database.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        /// url : /api/RiskProjects
        [HttpGet("/api/RiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProjectDto>>> GetRiskProjects()
        {
            var projects = _riskProjectQueries.QueryAllProjects();
            return Ok(await projects.ToListAsync());
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

            var riskProjects = await _riskProjectQueries.GetAllAdminRiskProjectsAsync(user);

            return Ok(riskProjects);
        }

        /// <summary>
        /// This controller method returns all information about a risk project with specific id.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject. </param>
        /// <returns> Returns DTO used for risk project page. </returns>
        /// url: api/RiskProjects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskProjectPageDto>> GetRiskProject(int id)
        {
            // TODO -> should be checking if wanted risk project IsBlank or not
            // if it is, then user ProjectManager should be prompted to set up project
            // In frontend I should check if project is blank and if it is, then I should redirect user to InitialRiskProjectSetup modal which will be unclosable
            var user = await _userManager.GetUserAsync(User);
            var riskProjectPage = await _riskProjectQueries.GetRiskProjectPageAsync(id, user.Id);

            if (riskProjectPage == null)
            {
                return NotFound();
            }

            riskProjectPage.IsAdmin = user.SystemRole.IsAdministrator; // TODO -> delete is just example

            return Ok(riskProjectPage);
        }

        /// <summary>
        /// This controller method returns infromation needed for project detail.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns>Returns DTO for risk project detail tab.</returns>
        [HttpGet("{id}/Detail")]
        public async Task<ActionResult<RiskProjectDetailDto>> GetRiskProjectDetail(int id)
        {
            var riskProject = await _riskProjectQueries.GetRiskProjectDetailAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }

            return Ok(riskProject);
        }

        [HttpGet("{id}/GetComments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            // TODO -> think about moving CommentDto out of RiskProjectDetailDto and add as separate part for page
            // I could load them separatly when update is needed which would be faster
            // But then when loading page it would need to do one more query -> think about it
            // As it is rn when I edit description or title, it will load comments as well
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }

            var comments = _riskProjectQueries.GetRiskProjectCommentsAsync(id).Result;

            return Ok(comments);
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// This controller method return all projects that are stored in database.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        /// url : /api/RiskProjects
        [HttpPost("/api/RiskProjects")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjects([FromBody] DtParams dtParams)
        {
            var query = _riskProjectQueries.QueryAllProjects();
            query = _riskProjectQueries.ApplyFilterQueryProjects(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var projects = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResult<RiskProjectDto>
            {
                Data = projects,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// This controller method serves for getting all projects where user has a role.
        /// </summary>
        /// 
        /// <returns> Returns DTOs used for showing info about projects in a table. </returns>
        /// url : /api/RiskProject/UserRiskProjects
        [HttpPost("UserRiskProjects")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserRiskProjects([FromBody] DtParams dtParams)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO -> user is not logged in
                return NoContent();
            }

            var query = _riskProjectQueries.QueryUsersProjects(user);
            query = _riskProjectQueries.ApplyFilterQueryProjects(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var projects = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResult<RiskProjectDto>
            {
                Data = projects,
                TotalRowCount = totalRowCount
            });
        }

        // POST: api/RiskProjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // url: api/RiskProject
        [HttpPost]
        public async Task<IActionResult> CreateProject(RiskProjectCreateDto riskProject)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!user.SystemRole.IsAdministrator)
            {
                return Unauthorized();
            }

            // TODO -> some data validation
            var chosenUser = await _context.Users.Where(u => u.Email == riskProject.Email).FirstOrDefaultAsync();
            if (chosenUser == null)
            {
                return NotFound();
            }

            var newRiskProject = new RiskProject
            {
                Title = riskProject.Title,
                Start = riskProject.Start, // TODO -> maybe delete dates and let project manager to set them
                End = riskProject.End,
                IsBlank = true,
                IsValid = true,
                UserId = user.Id
            };

            _context.RiskProjects.Add(newRiskProject);
            _context.SaveChanges();

            // TODO -> validate if project manager was chosen
            var newProjectRole = new ProjectRole
            {
                RiskProjectId = newRiskProject.Id,
                UserId = chosenUser.Id,
                RoleType = RoleType.ProjectManager,
                Name = "Projektový manažer",
                IsReqApproved = true
            };

            _context.ProjectRoles.Add(newProjectRole);
            _context.SaveChanges();

            return Ok();
        }

        // TODO -> AddComment and GetComments

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(int riskProjectId, string text) // TODO -> switch text to body -> use [FromBody]
        {
            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound();
            }

            var newComment = new Comment
            {
                RiskProjectId = riskProjectId,
                UserId = user.Id,
                Text = text,
                Created = DateTime.Now // TODO -> gives worng date!!
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="riskProjectDto"></param>
        /// <returns></returns>
        /// url: api/RiskProjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRiskProject(int id, RiskProjectDetailDto riskProjectDto)
        {
            // TODO -> rn is using RiskProjectDetailDto, but mby it should have its own DTO so we dont send comments 
            if (id != riskProjectDto.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }
            else if (!_riskProjectQueries.IsProjectManager(riskProject, user).Result)
            {
                return Unauthorized();
            }

            riskProject.Title = riskProjectDto.Title;
            riskProject.Start = riskProjectDto.Start;
            riskProject.End = riskProjectDto.End;
            riskProject.Description = riskProjectDto.Description;
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// This controller method is used for initial setup of a risk project.
        /// In initial setup, project manager has to specify scale of matrix and can additionaly specify more info.
        /// </summary>
        /// 
        /// <param name="id"> Id of the project. </param>
        /// <param name="riskProjectDto"> Updated project data. </param>
        /// <returns> Retruns DTO for project page, which will be rendered. </returns>
        [HttpPut("{id}/InitialRiskProjectSetup")]
        public async Task<IActionResult> InitialRiskProjectSetup(int id, RiskProjectDetailDto riskProjectDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound();
            }
            else if (riskProject.IsBlank == false)
            {
                return BadRequest();
            }
            else if (!_riskProjectQueries.IsProjectManager(riskProject, user).Result)
            {
                return Unauthorized();
            }

            riskProject.Scale = riskProjectDto.Scale;
            riskProject.IsBlank = false;
            riskProject.Start = riskProjectDto.Start;
            riskProject.End = riskProjectDto.End;
            riskProject.Description = riskProjectDto.Description;
            riskProject.Title = riskProjectDto.Title;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/RestoreProject")]
        public async Task<IActionResult> RestoreProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null || riskProject.IsValid == true)
            {
                return NotFound();
            }

            riskProject.IsValid = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/RiskProjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null || riskProject.IsValid == false)
            {
                return NotFound();
            }

            riskProject.IsValid = false; // Soft delete
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool RiskProjectExists(int id)
        {
            return _context.RiskProjects.Any(e => e.Id == id);
        }
    }
}
