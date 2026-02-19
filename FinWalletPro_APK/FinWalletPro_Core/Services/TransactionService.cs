using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionRepository _transactionRepo;
        private readonly AccountRepository _accountRepo;

        public TransactionService(TransactionRepository transactionRepo, AccountRepository accountRepo)
        {
            _transactionRepo = transactionRepo;
            _accountRepo = accountRepo;
        }

        public async Task TransferAsync(int senderId, int receiverId, decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Invalid amount.");

            var sender = await _accountRepo.GetByIdAsync(senderId);
            var receiver = await _accountRepo.GetByIdAsync(receiverId);

            if (sender == null || receiver == null)
                throw new Exception("Account not found.");

            if (sender.Balance < amount)
                throw new Exception("Insufficient balance.");

            sender.Balance -= amount;
            receiver.Balance += amount;

            var transaction = new Transaction
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Amount = amount,
                TransactionDate = DateTime.UtcNow,
                Status = "Completed"
            };

            await _transactionRepo.AddAsync(transaction);
            await _accountRepo.UpdateAsync(sender);
            await _accountRepo.UpdateAsync(receiver);
        }

        public async Task<IEnumerable<Transaction>> GetHistoryAsync(int accountId)
        {
            return await _transactionRepo.GetByAccountIdAsync(accountId);
        }
    }
}
