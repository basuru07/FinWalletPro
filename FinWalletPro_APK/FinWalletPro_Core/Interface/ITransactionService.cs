using System.Transactions;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public class ITransactionService
    {
        Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task<List<Transaction>> GetTransactionsAsync(int accountId);
    }
}
