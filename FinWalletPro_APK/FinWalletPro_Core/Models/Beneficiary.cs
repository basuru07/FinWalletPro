namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Beneficiary
    {
        public long BeneficiaryId { get; set; }
        public long AccountId { get; set; }
        public string? NickName { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryEmail { get; set; }
        public string? BeneficiaryPhone { get; set; }
        public string? BankName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigate property to Account 
        public Account? Account { get; set; }
    }
}
