using System.ComponentModel.DataAnnotations;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Register_RequestDto
    public class RegisterRequestDto
    {
        [Required][MaxLength(200)] public string? FullName { get; set; }
        [Required][EmailAddress][MaxLength(200)] public string? Email { get; set; }
        [Required][MinLength(8)] public string? Password { get; set; }
        [Required][Compare("Password")] public string? ConfirmPassword { get; set; }
        [Phone] public string? PhoneNumber { get; set; }
    }

    // Login_RequestDto
    public class LoginRequestDto
    {
        [Required][EmailAddress] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
    }

    // Refresh_Token_RequestDto
    public class RefreshTokenRequestDto
    {
        [Required] public string? RefreshToken { get; set; }
    }

    // Change_Password_RequestDto
    public class ChangePasswordRequestDto
    {
        [Required] public string? CurrentPassword { get; set; }
        [Required][MinLength(8)] public string? NewPassword { get; set; }
        [Required][Compare("NewPassword")] public string ConfirmNewPassword { get; set; }
    }

    // Auth_ResponseDto
    public class AuthResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
        public AccountSummaryDto Account { get; set; }
    }

    // Account_SummaryDto
    public class AccountSummaryDto
    {
        public long AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? AccountNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public string? Currency { get; set; }
        public string? AccountStatus { get; set; }
    }
}
