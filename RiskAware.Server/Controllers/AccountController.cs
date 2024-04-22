using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using System.Security.Claims;

namespace RiskAware.Server.Controllers
{
    /// <summary>
    /// Controller for managing user accounts related requests.
    /// </summary>
    /// <author>Dominik Pop</author>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Method for logging in user.
        /// </summary>
        /// <param name="model"> DTO containing login info. </param>
        /// <returns> Returns if login was successful or not. </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    bool isAdmin = false;
                    if (User.Identity is { IsAuthenticated: true })
                        isAdmin = User.IsInRole("Admin");
                    return Ok(new { isAdmin });
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Method for logging out user.
        /// </summary>
        /// <returns> Returns is user was logout or not. </returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Method for checking if user is logged in.
        /// </summary>
        /// <returns> Returns true if user is logged in. </returns>
        [HttpGet("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new
            {
                IsLoggedIn = User.Identity is { IsAuthenticated: true },
                Email = User.Identity?.Name,
                IsAdmin = User.IsInRole("Admin")
            });
        }
    }
}
