using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService) => _accountService = accountService;

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            var account = new Account
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };
            var created = await _accountService.RegisterAsync(account, request.Password);
            return Ok(new { created.AccountId, created.Email, created.FullName });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginRequest request)
        {
            var token = await _accountService.AuthenticateAsync(request.Email, request.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            var account = await _accountService.GetByEmailAsync(request.Email);
            return Ok(new AuthResponse
            {
                Token = token,
                Email = account!.Email,
                FullName = account.FullName
            });
        }
    }
}
