using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;
using Octopus.Client.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WalletDbContext _context;

        public AccountRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> GetByIdAsync(long accountId)
            => await _context.Accounts
                .Include(a => a.BankCards)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

        public async Task<Account> GetByEmailAsync(string email)
            => await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email.ToUpper() == email.ToUpper());

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
            => await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        public async Task<Account> GetByRefreshTokenAsync(string refreshToken)
            => await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

        public async Task<Account> UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
            => await _context.Accounts.ToListAsync();

        // ─── Bank Card Methods ────────────────────────────────────────────────
        public async Task<BankCard> AddBankCardAsync(BankCard card)
        {
            _context.BankCards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<IEnumerable<BankCard>> GetBankCardsAsync(long accountId)
            => await _context.BankCards
                .Where(c => c.AccountId == accountId && c.IsActive)
                .OrderByDescending(c => c.IsDefault)
                .ToListAsync();

        public async Task<bool> RemoveBankCardAsync(long accountId, long cardId)
        {
            var card = await _context.BankCards
                .FirstOrDefaultAsync(c => c.CardId == cardId && c.AccountId == accountId);
            if (card == null) return false;

            card.IsActive = false;
            card.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // If removed card was default, assign default to next active card
            if (card.IsDefault)
            {
                var nextCard = await _context.BankCards
                    .FirstOrDefaultAsync(c => c.AccountId == accountId && c.IsActive);
                if (nextCard != null)
                {
                    nextCard.IsDefault = true;
                    nextCard.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<bool> SetDefaultCardAsync(long accountId, long cardId)
        {
            var cards = await _context.BankCards
                .Where(c => c.AccountId == accountId && c.IsActive)
                .ToListAsync();

            foreach (var c in cards)
            {
                c.IsDefault = (c.CardId == cardId);
                c.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
