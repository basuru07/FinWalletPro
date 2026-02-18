namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    // ── Auth request/response helpers ──────────────────────────
    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDto? User { get; set; }
    }

    // Partial UserDto needed in Core (matches API DTO)
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ── Transaction command payloads ───────────────────────────
    public class TransferCommand
    {
        public Guid UserId { get; set; }
        public string? DestinationWalletNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Pin { get; set; }
    }

    public class DepositCommand
    {
        public Guid UserId { get; set; }
        public Guid BankCardId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class WithdrawCommand
    {
        public Guid UserId { get; set; }
        public Guid BankCardId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Pin { get; set; }
    }
}
