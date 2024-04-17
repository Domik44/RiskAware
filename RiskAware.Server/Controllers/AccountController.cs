using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new { IsLoggedIn = User.Identity is { IsAuthenticated: true } });
        }
    }
}
