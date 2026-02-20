
using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public interface ITransactionService
    {
        Task<Transaction> TransferAsync(long senderAccountId, string receiverAccountNumber, decimal amount, string description, string category);
        Task<Transaction> DepositAsync(long accountId, decimal amount, string description);
        Task<Transaction> WithdrawAsync(long accountId, decimal amount, string description);
        Task<Transaction> GetTransactionByIdAsync(long transactionId);
        Task<Transaction> GetTransactionByReferenceAsync(string reference);
        Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(long accountId, TransactionFilter filter);
        Task<IEnumerable<Transaction>> GetStatementAsync(long accountId, DateTime from, DateTime to);
        Task<bool> ReverseTransactionAsync(long transactionId, string reason);
        Task<decimal> GetTotalSentAsync(long accountId, DateTime from, DateTime to);
        Task<decimal> GetTotalReceivedAsync(long accountId, DateTime from, DateTime to);
    }

    public class TransactionFilter
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
}

