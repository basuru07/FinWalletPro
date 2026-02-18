namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    // Generic service result wrapper
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };

        public static ServiceResult<T> Fail(string message, List<string> errors = null)
            => new() { Success = false, Message = message, Errors = errors ?? new List<string>() };
    }

    // Wallet DTOs
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

    // BankCard DTOs
    public class BankCardDto
    {
        public Guid Id { get; set; }
        public string? CardHolderName { get; set; }
        public string? Last4Digits { get; set; }
        public string? CardType { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddBankCardDto
    {
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? CVV { get; set; }
        public bool SetAsDefault { get; set; }
    }

    // Beneficiary DTOs
    public class BeneficiaryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AddBeneficiaryDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
    }

    // Transaction DTOs
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

    public class TransactionHistoryDto
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TransactionDto>? Transactions { get; set; }
    }

    public class TransactionFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
