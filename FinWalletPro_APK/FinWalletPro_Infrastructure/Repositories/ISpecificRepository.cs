using FinWalletPro_APK.FinWalletPro_Core.Models;
using System;
using System.Transactions;
using Transaction = FinWalletPro_APK.FinWalletPro_Core.Models.Transaction;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    // User
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task<bool> EmailExistsAsync(string email);
        Task<User> GetWithWalletAsync(Guid userId);
    }

    // Wallet
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet> GetByUserIdAsync(Guid userId);
        Task<Wallet> GetWalletNumberAsync(string walletNumber);
        Task<decimal> GetBalanceAsync(Guid walletId);
    }

    // Transaction
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, int pageNumber, int pageSize);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid walletId, DateTime start, DateTime end);
        Task<Transaction> GetByReferenceAsync(string reference);
        Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status);
        Task<int> CountByWalletAsync(Guid walletId);
    }

    public interface IBankCardRepository : IRepository<BankCard>
    {
        Task<IEnumerable<BankCard>> GetByUserIdAsync(Guid userId);
        Task<BankCard> GetDefaultCardAsync(Guid userId);
        Task SetDefaultCardAsync(Guid userId, Guid cardId);
    }

    public interface IBeneficiaryRepository : IRepository<Beneficiary>
    {
        Task<IEnumerable<Beneficiary>> GetByUserIdAsync(Guid userId);
        Task<Beneficiary> GetByWalletNumberAsync(Guid userId, string walletNumber);
    }
}
