namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Beneficiary
    {
        public int BeneficiaryId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WalletAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
