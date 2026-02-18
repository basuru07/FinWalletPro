using static FinWalletPro_APK.FinWalletPro_Core.Models.Domainenums;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // ------------------ Wallet DTO ------------------
    public class WalletDto
    {
        public Guid Id { get; set; }
        public string? WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
    }

    public class WalletBalanceDto
    {
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal PendingAmount { get; set; }
        public string? Currency { get; set; }
    }

    // ------------------ Transaction DTO ------------------
    public class TransactionDto 
    {
        public Guid Id { get; set; }
        public string? TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateTransactionDto
    {
        public Guid? DestinationWalletId { get; set; }
        public string? DestinationWalletNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Pin {  get; set; }
    }

    public class TransactionHistoryDto
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TransactionDto>? Transactions { get; set; }
    }

    public class TransactionFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TransactionType? Type { get; set; }
        public TransactionStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class DepositRequestDto
    {
        public Guid BankCardId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class WithdrawalRequestDto
    {
        public Guid BankCardId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Pin { get; set; }
    }
}
