using System.ComponentModel.DataAnnotations;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // ─── Request DTOs ─────────────────────────────────────────────────────────
    public class UpdateAccountRequestDto
    {
        [MaxLength(200)] public string? FullName { get; set; }
        [Phone] public string? PhoneNumber { get; set; }
    }

    public class AddBankCardRequestDto
    {
        [Required][MaxLength(200)] public string? CardHolderName { get; set; }
        [Required][CreditCard] public string? CardNumber { get; set; }
        [Required][RegularExpression(@"^(0[1-9]|1[0-2])$")] public string ExpiryMonth { get; set; }
        [Required][RegularExpression(@"^\d{4}$")] public string ExpiryYear { get; set; }
        [Required][MaxLength(50)] public string? CardType { get; set; }      // Visa, MasterCard, Amex
        [Required][MaxLength(20)] public string? CardCategory { get; set; }  // Credit, Debit
        [MaxLength(100)] public string? BankName { get; set; }
    }

    // ─── Response DTOs ────────────────────────────────────────────────────────
    public class AccountDetailDto
    {
        public long AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string? Currency { get; set; }
        public string? AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<BankCardDto> BankCards { get; set; }
    }

    public class BalanceResponseDto
    {
        public long AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string? Currency { get; set; }
        public DateTime AsOf { get; set; }
    }

    public class BankCardDto
    {
        public long CardId { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardNumberMasked { get; set; }
        public string? CardType { get; set; }
        public string? CardCategory { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? BankName { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LinkedAt { get; set; }
    }
}
