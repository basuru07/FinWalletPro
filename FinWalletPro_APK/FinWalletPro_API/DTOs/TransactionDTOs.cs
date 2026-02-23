using System.ComponentModel.DataAnnotations;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Transfer_RequestDto
    public class TransferRequestDto
    {
        [Required] public string? ReceiverAccountNumber { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        [MaxLength(500)] public string? Description { get; set; }
        [MaxLength(100)] public string? Category { get; set; }
    }

    // Deposit_RequestDto
    public class DepositRequestDto
    {
        [Required][Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
        [MaxLength(500)] public string? Description { get; set; }
    }

    // Withdraw_RequestDto
    public class WithdrawRequestDto
    {
        [Required][Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
        [MaxLength(500)] public string? Description { get; set; }
    }

    // Transaction_Filter_RequestDto
    public class TransactionFilterRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TransactionType { get; set; }
        public string? Status { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    // Statement_RequestDto
    public class StatementRequestDto
    {
        [Required] public DateTime FromDate { get; set; }
        [Required] public DateTime ToDate { get; set; }
    }

    // Reverse_Transaction_RequestDto
    public class ReverseTransactionRequestDto
    {
        [Required][MaxLength(500)] public string? Reason { get; set; }
    }

    // ─── Response DTOs ────────────────────────────────────────────────────────
    public class TransactionResponseDto
    {
        public long TransactionId { get; set; }
        public string? TransactionReference { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAccountNumber { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string? TransactionType { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class TransactionSummaryDto
    {
        public int TotalTransactions { get; set; }
        public decimal TotalSent { get; set; }
        public decimal TotalReceived { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
