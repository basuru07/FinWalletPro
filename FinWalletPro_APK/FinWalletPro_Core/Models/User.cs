namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string? IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Navigation Properties
        public virtual Wallet Wallet { get; set; }
        public virtual ICollection<BankCard> BankCards { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            CreateAt = DateTime.UtcNow;
            IsActive = true;
            BankCards = new HashSet<BankCard>();
            Beneficiaries = new HashSet<Beneficiary>();
        }

    }
}
