namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class BankCard
    {
        public long CardId { get; set; }
        public long AccountId { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardNumberMasked { get; set; }  // Store only last 4 digits masked
        public string? CardNumberHash { get; set; }    // Hash of full card number
        public string? CardType { get; set; }          // Visa, MasterCard, Amex
        public string? CardCategory { get; set; }      // Credit, Debit
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? BankName { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime LinkedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Account? Account { get; set; }
    }
}
