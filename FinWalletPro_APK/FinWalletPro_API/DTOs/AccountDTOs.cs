namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
