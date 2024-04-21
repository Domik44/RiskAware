using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;
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
        private readonly ProjectRoleQueries _projectRoleQueries;

        public RiskProjectController(AppDbContext context, UserManager<User> userManager, RiskProjectQueries riskProjectQueries, ProjectRoleQueries projectRoleQueries)
        {
            _context = context;
            _userManager = userManager;
            _riskProjectQueries = riskProjectQueries;
            _projectRoleQueries = projectRoleQueries;
        }

        ////////////////// GET METHODS //////////////////

        /// <summary>
        /// Method for getting risk project page.
        /// </summary>
        /// <param name="id"> Id of a RiskProject. </param>
        /// <returns> Returns DTO used for risk project page, contains all necessary info. </returns>
        /// url: api/RiskProjects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskProjectPageDto>> GetRiskProject(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var riskProjectPage = await _riskProjectQueries.GetRiskProjectPageAsync(id, user.Id);

            if (riskProjectPage == null)
            {
                return NotFound();
            }

            return Ok(riskProjectPage);
        }

        /// <summary>
        /// Method for getting detail of a risk project.
        /// </summary>
        /// 
        /// <param name="id"> Id of a RiskProject </param>
        /// <returns> Returns DTO for risk project detail tab. </returns>
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

        /// <summary>
        /// Method for getting comments of a risk project.
        /// </summary>
        /// <param name="id"> Risk project id. </param>
        /// <returns> Returns a list of DTOs containing basic info about comment. </returns>
        [HttpGet("{id}/GetComments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null)
            {
                return NotFound("Risk project not found!");
            }

            var comments = _riskProjectQueries.GetRiskProjectCommentsAsync(id).Result;

            return Ok(comments);
        }

        ////////////////// POST METHODS //////////////////
        /// <summary>
        /// Get filtered risk projects.
        /// </summary>
        /// <param name="dtParams"> Data table filtering parameters. </param>
        /// <returns> Filtered projects DTOs </returns>
        [HttpPost("/api/RiskProjects")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRiskProjects([FromBody] DtParamsDto dtParams)
        {
            var query = _riskProjectQueries.QueryAllProjects();
            query = _riskProjectQueries.ApplyFilterQueryProjects(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var projects = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();

            return new JsonResult(new DtResultDto<RiskProjectDto>
            {
                Data = projects,
                TotalRowCount = totalRowCount,
            });
        }

        /// <summary>
        /// Get filtered user participated risk projects
        /// </summary>
        /// <param name="dtParams">Data table filtering parameters</param>
        /// <returns>Filtered projects DTOs</returns>
        /// <url>/api/RiskProject/UserRiskProjects</url>
        [HttpPost("UserRiskProjects")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserRiskProjects([FromBody] DtParamsDto dtParams)
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
            return new JsonResult(new DtResultDto<RiskProjectDto>
            {
                Data = projects,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for creating new risk project by admin.
        /// </summary>
        /// <param name="riskProject"> DTO containing info about created project. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        [HttpPost]
        public async Task<IActionResult> CreateProject(RiskProjectCreateDto riskProject)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!user.SystemRole.IsAdministrator)
            {
                return Unauthorized("User is not admin!");
            }

            var chosenUser = await _context.Users.Where(u => u.Email == riskProject.Email).FirstOrDefaultAsync();
            if (chosenUser == null)
            {
                return NotFound("Project manager not found!");
            }

            var newRiskProject = new RiskProject
            {
                Title = riskProject.Title,
                Start = riskProject.Start,
                End = riskProject.End,
                IsBlank = true,
                IsValid = true,
                UserId = user.Id
            };

            _context.RiskProjects.Add(newRiskProject);
            await _context.SaveChangesAsync();

            var newProjectRole = new ProjectRole
            {
                RiskProjectId = newRiskProject.Id,
                UserId = chosenUser.Id,
                RoleType = RoleType.ProjectManager,
                Name = "Projektový manažer",
                IsReqApproved = true
            };

            _context.ProjectRoles.Add(newProjectRole);
            await _context.SaveChangesAsync();

            var b = _riskProjectQueries.CreateDefaultCategories(newRiskProject);

            return Ok();
        }

        /// <summary>
        /// Method for adding comment to a risk project.
        /// </summary>
        /// <param name="riskProjectId"> Id of risk project. </param>
        /// <param name="text"> Text of comment. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(int riskProjectId, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(riskProjectId);
            if (riskProject == null)
            {
                return NotFound("Project not found!");
            }

            var isCommonUser = await _projectRoleQueries.IsCommonUser(riskProjectId, user.Id);
            if (isCommonUser) // User has no role on project.
            {
                return Unauthorized("User cannot add comments.");
            }

            var newComment = new Comment
            {
                RiskProjectId = riskProjectId,
                UserId = user.Id,
                Text = text,
                Created = DateTime.Now
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// PUT METHODS //////////////////

        /// <summary>
        /// Method for editing risk project detail.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <param name="riskProjectDto"> DTO containing basic info about risk project. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/RiskProjects/5
        // TODO -> frontend
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRiskProject(int id, RiskProjectDetailDto riskProjectDto)
        {
            if (id != riskProjectDto.Id)
            {
                return BadRequest("Risk project ids dont match!");
            }

            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(id);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(riskProject.Id, user.Id);
            if (riskProject == null)
            {
                return NotFound("Risk project not found!");
            }
            else if (!isProjectManager)
            {
                return Unauthorized("Only project manager can edit project detail!");
            }

            riskProject.Title = riskProjectDto.Title;
            riskProject.Start = riskProjectDto.Start;
            riskProject.End = riskProjectDto.End;
            riskProject.Description = riskProjectDto.Description;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// This method is used for initial setup of a risk project.
        /// In initial setup, project manager has to specify scale of matrix and can additionaly specify more info.
        /// </summary>
        /// <param name="id"> Id of the project. </param>
        /// <param name="riskProjectDto"> Updated project data. </param>
        /// <returns> Retruns DTO for project page, which will be rendered. </returns>
        /// url: api/RiskProjects/5/InitialRiskProjectSetup
        [HttpPut("{id}/InitialRiskProjectSetup")]
        public async Task<IActionResult> InitialRiskProjectSetup(int id, RiskProjectDetailDto riskProjectDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var riskProject = await _context.RiskProjects.FindAsync(id);
            var isProjectManager = await _projectRoleQueries.IsProjectManager(riskProject.Id, user.Id);
            if (riskProject == null)
            {
                return NotFound("Risk project not found!");
            }
            else if (riskProject.IsBlank == false)
            {
                return BadRequest("Risk project is already set!");
            }
            else if (!isProjectManager)
            {
                return Unauthorized("User is not a project manager.");
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

        /// <summary>
        /// Method for restoring deleted risk project.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/RiskProjects/5/RestoreProject
        // TODO -> frontend
        [HttpPut("{id}/RestoreProject")]
        public async Task<IActionResult> RestoreProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null || riskProject.IsValid == true)
            {
                return NotFound("Risk project not found!");
            }

            riskProject.IsValid = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// Method for deleting risk project.
        /// </summary>
        /// <param name="id"> Id of risk project. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/RiskProjects/5
        // TODO -> frontend 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRiskProject(int id)
        {
            var riskProject = await _context.RiskProjects.FindAsync(id);
            if (riskProject == null || riskProject.IsValid == false)
            {
                return NotFound("Risk project not found!");
            }

            riskProject.IsValid = false; // Soft delete
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
