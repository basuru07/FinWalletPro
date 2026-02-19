namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
