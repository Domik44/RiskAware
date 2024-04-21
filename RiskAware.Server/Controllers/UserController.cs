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

        /// <summary>
        /// Method for getting all users in system.
        /// </summary>
        /// <returns> Returns list of User DTOs, which containt basic user info. </returns>
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
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Method for getting info about user account.
        /// </summary>
        /// <param name="id"> Id of user we want to get. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/User/{id}
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

        /// <summary>
        /// Method for creating new user account by admin.
        /// </summary>
        /// <param name="userDto"> DTO containing info about new user. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/User
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
                UserName = userDto.Email,
                SystemRoleId = 2
            };

            var result = await _userManager.CreateAsync(user, "Basic123");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Method for updating basic info about user account.
        /// Update can be done only by user itself or by admin.
        /// </summary>
        /// <param name="id"> Id of user which info we want to update. </param>
        /// <param name="userDto"> DTO containing new info about user. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/User/{id}
        // TODO TEST -> check testing method
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDetailDto userDto)
        {
            var activeUser = await _userManager.GetUserAsync(User); // get logged user
            User user;
            if (!activeUser.SystemRole.IsAdministrator && id != activeUser.Id) // only admin / user itself can update info
            {
                return BadRequest("Id of given user does not match!");
            }
            else
            {
                if (activeUser.SystemRole.IsAdministrator)
                {
                    user = await _context.Users.FindAsync(id);
                }
                else
                {
                    user = activeUser;
                }
            }

            if (user == null)
            {
                return NotFound("User not found!");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        /// <summary>
        /// Method for changing password of user account.
        /// </summary>
        /// <param name="id"> Id of user which is changing its password. </param>
        /// <param name="passwordDto"> DTO containing passwords. </param>
        /// <returns> Returns if action was succesful or not. </returns>
        /// url: api/User/{id}/ChangePassword
        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(string id, PasswordDto passwordDto)
        {
            if (string.IsNullOrEmpty(id) || passwordDto == null)
            {
                return BadRequest("Invalid parameters");
            }

            var activeUser = await _userManager.GetUserAsync(User);
            if (activeUser.Id != id)
            {
                return NotFound("Ids dont match!");
            }

            var res = await _userManager.ChangePasswordAsync(activeUser, passwordDto.OldPassword, passwordDto.NewPassword);
            if (!res.Succeeded)
            {
                return BadRequest(res.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Method for restoring user account. It only sets IsValid to true.
        /// </summary>
        /// <param name="id"> Id of user we want to restore. </param>
        /// <returns> Returns if action was succesfull or not. </returns>
        /// url: api/User/{id}/Restore
        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.IsValid = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Method for deleting user account. It only sets IsValid to false.
        /// Does soft delete -> makes user account invalid.
        /// </summary>
        /// <param name="id"> Id of user we want to delete. </param>
        /// <returns> Returns if action was succesfull or not. </returns>
        /// url: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.IsValid = false;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
