using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account account);
        Task<Account> GetByIdAsync(long accountId);
        Task<Account> GetByEmailAsync(string email);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
        Task<Account> GetByRefreshTokenAsync(string refreshToken);
        Task<Account> UpdateAsync(Account account);
        Task<IEnumerable<Account>> GetAllAsync();

        Task<BankCard> AddBankCardAsync(BankCard card);
        Task<IEnumerable<BankCard>> GetBankCardsAsync(long accountId);
        Task<bool> RemoveBankCardAsync(long accountId, long cardId);
        Task<bool> SetDefaultCardAsync(long accountId, long cardId);
    }
}
