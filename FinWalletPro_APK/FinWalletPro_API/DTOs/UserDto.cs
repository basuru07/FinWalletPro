namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateTime CreateAt { get; set; }
        public WalletDto Wallet { get; set; }
    }

    public class WalletDto
    {
        public Guid Id { get; set; }
        public string? WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
    }
}
