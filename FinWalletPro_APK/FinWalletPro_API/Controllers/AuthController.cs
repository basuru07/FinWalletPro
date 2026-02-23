using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        // Constructor
        public AuthController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        // Register n new wallet account
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var account = new Account
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var created = await _accountService.RegisterAsync(account, dto.Password);

            return StatusCode(201, new ApiResponse<AccountSummaryDto>
            {
                Success = true,
                Message = "Account registered successfully.",
                Data = MapToAccountSummary(created)
            });
        }

        // Login and obtain JWT tokens
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var (account, accessToken, refreshToken) = await _accountService.LoginAsync(dto.Email, dto.Password);
            int expiresIn = Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"]) * 60;

            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = expiresIn,
                    Account = MapToAccountSummary(account)
                }
            });
        }

        // Refresh expired access token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var (newAccess, newRefresh) = await _accountService.RefreshTokenAsync(dto.RefreshToken);
            int expiresIn = Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"]) * 60;

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Token refreshed successfully.",
                Data = new { accessToken = newAccess, refreshToken = newRefresh, expiresIn }
            });
        }

        // Logout and invalidate refresh token
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var accountId = GetCurrentAccountId();
            await _accountService.LogoutAsync(accountId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Logged out successfully." });
        }

        // Change account password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            await _accountService.ChangePasswordAsync(accountId, dto.CurrentPassword, dto.NewPassword);
            return Ok(new ApiResponse<object> { Success = true, Message = "Password changed successfully." });
        }

        // ─── Helpers ──────────────────────────────────────────────────────────
        private long GetCurrentAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("Invalid token claims.");
            return long.Parse(claim.Value);
        }

        private static AccountSummaryDto MapToAccountSummary(Account a) => new()
        {
            AccountId = a.AccountId,
            FullName = a.FullName,
            Email = a.Email,
            AccountNumber = a.AccountNumber,
            PhoneNumber = a.PhoneNumber,
            Balance = a.Balance,
            Currency = a.Currency,
            AccountStatus = a.AccountStatus
        };
    }
}
