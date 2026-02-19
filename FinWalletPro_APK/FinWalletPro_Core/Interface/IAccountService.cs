using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public class IAccountService
    {
        Task<Account> RegisterAsync(Account account, string password);
        Task<Account?> GetByIdAsync(int accountId);
        Task<Account?> GetByEmailAsync(string email);
        Task<decimal> GetBalanceAsync(int accountId);
        Task<string?> AuthenticateAsync(string email, string password);
    }
}
