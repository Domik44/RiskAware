using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.SystemRole.IsAdministrator)
            {
                Unauthorized();
            }

            var users = await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName,
                Email = u.Email
            }).ToListAsync();

            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> GetUser(string id)
        {
            var user = await _context.Users.Where(u => u.Id == id)
                .Select(u => new UserDetailDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserDetailDto userDto)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if (!activeUser.SystemRole.IsAdministrator)
            {
                return Unauthorized();
            }

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
            };

            await _userManager.CreateAsync(user, "Basic123"); // TODO -> prolly cahnge this so admin can set random passwords

            return Ok();
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDetailDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            // TODO -> implement this method
            return BadRequest();
        }

        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsValid = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsValid = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
