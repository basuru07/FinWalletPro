using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;
using Octopus.Client.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WalletDbContext _context;
        public AccountRepository(WalletDbContext context) { _context = context; }

        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> GetByIdAsync(int id)
            => await _context.Accounts.FindAsync(id);

        public async Task<Account?> GetByEmailAsync(string email)
            => await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
    }
}
