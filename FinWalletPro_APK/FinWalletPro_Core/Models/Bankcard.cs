namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class BankCard
    {
        public int CardId { get; set; }
        public int AccountId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string CardType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
