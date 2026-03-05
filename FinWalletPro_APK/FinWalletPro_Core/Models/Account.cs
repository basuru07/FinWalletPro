using System.ComponentModel.DataAnnotations.Schema;

namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Account
    {
        public long AccountId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
        public string Currency { get; set; } = "USD";
        public string AccountStatus { get; set; } = "Active"; // Active, Suspended, Closed
        public string? AccountNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("UPDATE_AT")]
        public DateTime? UpdatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }


        // Interface classes
        public ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();
        public ICollection<Transaction>? SentTransactions { get; set; } 
        public ICollection<Transaction>? ReceivedTransactions { get; set; }
        public ICollection<Beneficiary>? Beneficiaries { get; set; }
        public ICollection<Notification?> Notifications { get; set; }
    }
}
