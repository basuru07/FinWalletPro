using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction> GetByIdAsync(long transactionId);
        Task<Transaction> GetByReferenceAsync(string reference);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(long accountId, TransactionFilter filter);
        Task<IEnumerable<Transaction>> GetStatementAsync(long accountId, DateTime from, DateTime to);
        Task<decimal> GetTotalSentAsync(long accountId, DateTime from, DateTime to);
        Task<decimal> GetTotalReceivedAsync(long accountId, DateTime from, DateTime to);
       
    }
}
