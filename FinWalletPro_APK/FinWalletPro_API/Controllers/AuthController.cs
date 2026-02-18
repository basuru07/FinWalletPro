using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new RegisterRequest
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password
            };

            var result = await _userService.RegisterAsync(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginRequest = new LoginRequest
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _userService.LoginAsync(loginRequest);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        // POST api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT is stateless — client drops the token
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
