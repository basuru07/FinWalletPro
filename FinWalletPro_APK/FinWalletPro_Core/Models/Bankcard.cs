namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class BankCard
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; } // Encrypted
        public string Last4Digits { get; set; }
        public string? CardType { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set;}
        public bool IsDefault {  get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        
        // Navigation Properties
        public virtual User User { get; set; }

        
        // BankCard Constructor
        public BankCard()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }

    public class Beneficiary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        
        // Navigation Properties
        public virtual User User { get; set; }

        public Beneficiary()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
