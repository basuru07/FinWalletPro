using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;
        private const decimal TransferFeeRate = 0.005m; // 0.5% fee

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            INotificationService notificationService)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public async Task<Transaction> TransferAsync(long senderAccountId, string receiverAccountNumber, decimal amount, string description, string category)
        {
            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero.");

            var sender = await _accountRepository.GetByIdAsync(senderAccountId)
                ?? throw new KeyNotFoundException("Sender account not found.");

            var receiver = await _accountRepository.GetByAccountNumberAsync(receiverAccountNumber)
                ?? throw new KeyNotFoundException("Receiver account not found.");

            if (sender.AccountId == receiver.AccountId)
                throw new InvalidOperationException("Cannot transfer to your own account.");

            if (sender.AccountStatus != "Active")
                throw new InvalidOperationException("Sender account is not active.");

            if (receiver.AccountStatus != "Active")
                throw new InvalidOperationException("Receiver account is not active.");

            var fee = Math.Round(amount * TransferFeeRate, 2);
            var totalDeduction = amount + fee;

            if (sender.Balance < totalDeduction)
                throw new InvalidOperationException($"Insufficient balance. Required: {totalDeduction:C}, Available: {sender.Balance:C}");

            // Deduct from sender
            sender.Balance -= totalDeduction;
            sender.UpdatedAt = DateTime.UtcNow;

            // Credit receiver
            receiver.Balance += amount;
            receiver.UpdatedAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                TransactionReference = GenerateReference(),
                SenderAccountId = senderAccountId,
                ReceiverAccountId = receiver.AccountId,
                Amount = amount,
                Currency = sender.Currency,
                TransactionType = "Transfer",
                Status = "Completed",
                Description = description,
                Category = category ?? "Transfer",
                Fee = fee,
                BalanceAfter = sender.Balance,
                TransactionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            };

            await _accountRepository.UpdateAsync(sender);
            await _accountRepository.UpdateAsync(receiver);
            var savedTransaction = await _transactionRepository.CreateAsync(transaction);

            // Notifications
            await _notificationService.SendTransactionNotificationAsync(
                senderAccountId,
                "Transfer Sent",
                $"You sent {amount:C} to {receiver.FullName}. Ref: {transaction.TransactionReference}",
                savedTransaction.TransactionId.ToString());

            await _notificationService.SendTransactionNotificationAsync(
                receiver.AccountId,
                "Transfer Received",
                $"You received {amount:C} from {sender.FullName}. Ref: {transaction.TransactionReference}",
                savedTransaction.TransactionId.ToString());

            return savedTransaction;
        }

        public async Task<Transaction> DepositAsync(long accountId, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            var account = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found.");

            account.Balance += amount;
            account.UpdatedAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                TransactionReference = GenerateReference(),
                SenderAccountId = accountId,
                ReceiverAccountId = accountId,
                Amount = amount,
                Currency = account.Currency,
                TransactionType = "Deposit",
                Status = "Completed",
                Description = description ?? "Deposit",
                Category = "Deposit",
                Fee = 0,
                BalanceAfter = account.Balance,
                TransactionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            };

            await _accountRepository.UpdateAsync(account);
            var saved = await _transactionRepository.CreateAsync(transaction);

            await _notificationService.SendTransactionNotificationAsync(
                accountId,
                "Deposit Successful",
                $"{amount:C} has been deposited to your account. New Balance: {account.Balance:C}",
                saved.TransactionId.ToString());

            return saved;
        }

        public async Task<Transaction> WithdrawAsync(long accountId, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            var account = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found.");

            if (account.Balance < amount)
                throw new InvalidOperationException($"Insufficient balance. Available: {account.Balance:C}");

            account.Balance -= amount;
            account.UpdatedAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                TransactionReference = GenerateReference(),
                SenderAccountId = accountId,
                ReceiverAccountId = accountId,
                Amount = amount,
                Currency = account.Currency,
                TransactionType = "Withdrawal",
                Status = "Completed",
                Description = description ?? "Withdrawal",
                Category = "Withdrawal",
                Fee = 0,
                BalanceAfter = account.Balance,
                TransactionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            };

            await _accountRepository.UpdateAsync(account);
            var saved = await _transactionRepository.CreateAsync(transaction);

            await _notificationService.SendTransactionNotificationAsync(
                accountId,
                "Withdrawal Processed",
                $"{amount:C} has been withdrawn from your account. New Balance: {account.Balance:C}",
                saved.TransactionId.ToString());

            return saved;
        }

        public async Task<Transaction> GetTransactionByIdAsync(long transactionId)
        {
            var tx = await _transactionRepository.GetByIdAsync(transactionId);
            if (tx == null) throw new KeyNotFoundException("Transaction not found.");
            return tx;
        }

        public async Task<Transaction> GetTransactionByReferenceAsync(string reference)
        {
            var tx = await _transactionRepository.GetByReferenceAsync(reference);
            if (tx == null) throw new KeyNotFoundException("Transaction not found.");
            return tx;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(long accountId, TransactionFilter filter)
            => await _transactionRepository.GetByAccountIdAsync(accountId, filter);

        public async Task<IEnumerable<Transaction>> GetStatementAsync(long accountId, DateTime from, DateTime to)
            => await _transactionRepository.GetStatementAsync(accountId, from, to);

        public async Task<bool> ReverseTransactionAsync(long transactionId, string reason)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);

            if (transaction.Status != "Completed")
                throw new InvalidOperationException("Only completed transactions can be reversed.");

            if (transaction.TransactionType != "Transfer")
                throw new InvalidOperationException("Only transfer transactions can be reversed.");

            // Reverse the money movement
            var sender = await _accountRepository.GetByIdAsync(transaction.SenderAccountId);
            var receiver = await _accountRepository.GetByIdAsync(transaction.ReceiverAccountId);

            if (receiver.Balance < transaction.Amount)
                throw new InvalidOperationException("Receiver has insufficient balance for reversal.");

            receiver.Balance -= transaction.Amount;
            sender.Balance += transaction.Amount + transaction.Fee;
            sender.UpdatedAt = DateTime.UtcNow;
            receiver.UpdatedAt = DateTime.UtcNow;

            transaction.Status = "Reversed";

            await _accountRepository.UpdateAsync(sender);
            await _accountRepository.UpdateAsync(receiver);
            await _transactionRepository.UpdateAsync(transaction);

            await _notificationService.SendTransactionNotificationAsync(
                transaction.SenderAccountId,
                "Transaction Reversed",
                $"Transaction {transaction.TransactionReference} has been reversed. Reason: {reason}",
                transactionId.ToString());

            return true;
        }

        public async Task<decimal> GetTotalSentAsync(long accountId, DateTime from, DateTime to)
            => await _transactionRepository.GetTotalSentAsync(accountId, from, to);

        public async Task<decimal> GetTotalReceivedAsync(long accountId, DateTime from, DateTime to)
            => await _transactionRepository.GetTotalReceivedAsync(accountId, from, to);

        private static string GenerateReference()
        {
            var rng = new Random();
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{rng.Next(1000, 9999)}";
        }
    }
}
