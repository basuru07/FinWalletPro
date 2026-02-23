using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public interface IAccountService
    {
        Task<Account> RegisterAsync(Account account, string password);
        Task<(Account account, string accessToken, string refreshToken)> LoginAsync(string email, string password);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(long accountId);
        Task<Account> GetAccountByIdAsync(long accountId);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Account> UpdateAccountAsync(long accountId, Account updatedAccount);
        Task<bool> ChangePasswordAsync(long accountId, string currentPassword, string newPassword);
        Task<decimal> GetBalanceAsync(long accountId);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<bool> SuspendAccountAsync(long accountId);
        Task<bool> ActivateAccountAsync(long accountId);

        
        // Bank Card
        Task<BankCard> AddBankCardAsync(BankCard card);
        Task<IEnumerable<BankCard>> GetBankCardsAsync(long accountId);
        Task<bool> RemoveBankCardAsync(long accountId, long cardId);
        Task<bool> SetDefaultCardAsync(long accountId, long cardId);
    }
}
