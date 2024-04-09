using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
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

        // GET: api/RiskProjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RiskProject>>> GetRiskProjects()
        {
            return await _context.RiskProjects.ToListAsync();
        }

        // GET: api/UserRiskProjects
        [HttpGet("UserRiskProjects")]
        public async Task<ActionResult<IEnumerable<RiskProject>>> GetUserRiskProjects()
        {
            // TODO -> poresit logiku s logged userem
            //var user = User.Identity;
            var user = await _context.Users.FindAsync("d6f46418-2c21-43f8-b167-162fb5e3a999"); // TODO -> for swagger testing purposes

            if (user == null)
            {
                //TODO
                return NoContent();
            }

            // TODO -> logika pro admina
            //if(user.SystemRole.IsAdministrator == true)
            //{

            //}
            //else
            //{

            // TODO -> slozita logika, kde budeme muset delat joiny pomoci projectRole
            //var projects = _context.RiskProjects.Where(u => u.Use)

            //}
            return await _context.RiskProjects.ToListAsync();
        }

        // GET: api/RiskProjects/5
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
