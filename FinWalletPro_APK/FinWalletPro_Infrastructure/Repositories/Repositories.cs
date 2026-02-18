using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _db;

        public Repository(AppDbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
            => await _db.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _db.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _db.Where(predicate).ToListAsync();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _db.FirstOrDefaultAsync(predicate);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _db.AnyAsync(predicate);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
            => predicate == null
                ? await _db.CountAsync()
                : await _db.CountAsync(predicate);

        public async Task<T> AddAsync(T entity)
        {
            await _db.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _db.AddRangeAsync(entities);

        public Task UpdateAsync(T entity)
        {
            _db.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _db.Remove(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _db.Remove(entity);
            return Task.CompletedTask;
        }
    }

    // ─── User Repository ──────────────────────────────────────
    public class UserRepository : Repository<FinWalletPro_Core.Models.User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<FinWalletPro_Core.Models.User> GetByEmailAsync(string email)
            => await _db.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<FinWalletPro_Core.Models.User> GetByPhoneNumberAsync(string phone)
            => await _db.FirstOrDefaultAsync(u => u.PhoneNumber == phone);

        public async Task<bool> EmailExistsAsync(string email)
            => await _db.AnyAsync(u => u.Email == email);

        public async Task<FinWalletPro_Core.Models.User> GetWithWalletAsync(Guid userId)
            => await _db.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Id == userId);
    }

    // ─── Wallet Repository ────────────────────────────────────
    public class WalletRepository : Repository<FinWalletPro_Core.Models.Wallet>, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context) { }

        public async Task<FinWalletPro_Core.Models.Wallet> GetByUserIdAsync(Guid userId)
            => await _db.FirstOrDefaultAsync(w => w.UserId == userId);

        public async Task<FinWalletPro_Core.Models.Wallet> GetByWalletNumberAsync(string number)
            => await _db.FirstOrDefaultAsync(w => w.WalletNumber == number);

        public async Task<decimal> GetBalanceAsync(Guid walletId)
        {
            var wallet = await _db.FindAsync(walletId);
            return wallet?.Balance ?? 0;
        }
    }

    // ─── Transaction Repository ───────────────────────────────
    public class TransactionRepository : Repository<FinWalletPro_Core.Models.Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<FinWalletPro_Core.Models.Transaction>> GetByWalletIdPagedAsync(
            Guid walletId, int page, int size)
            => await _db
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * size).Take(size)
                .ToListAsync();

        public async Task<IEnumerable<FinWalletPro_Core.Models.Transaction>> GetByDateRangeAsync(
            Guid walletId, DateTime start, DateTime end)
            => await _db
                .Where(t => (t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                         && t.CreatedAt >= start && t.CreatedAt <= end)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<FinWalletPro_Core.Models.Transaction> GetByReferenceAsync(string reference)
            => await _db.FirstOrDefaultAsync(t => t.TransactionReference == reference);

        public async Task<IEnumerable<FinWalletPro_Core.Models.Transaction>> GetByStatusAsync(
            FinWalletPro_Core.Models.Enums.TransactionStatus status)
            => await _db.Where(t => t.Status == status).ToListAsync();

        public async Task<int> CountByWalletAsync(Guid walletId)
            => await _db.CountAsync(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId);
    }

    // ─── BankCard Repository ──────────────────────────────────
    public class BankCardRepository : Repository<FinWalletPro_Core.Models.BankCard>, IBankCardRepository
    {
        public BankCardRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<FinWalletPro_Core.Models.BankCard>> GetActiveCardsByUserIdAsync(Guid userId)
            => await _db.Where(c => c.UserId == userId && c.IsActive).ToListAsync();

        public async Task<FinWalletPro_Core.Models.BankCard> GetDefaultCardAsync(Guid userId)
            => await _db.FirstOrDefaultAsync(c => c.UserId == userId && c.IsDefault && c.IsActive);

        public async Task SetDefaultCardAsync(Guid userId, Guid cardId)
        {
            // Unset existing default
            var cards = await _db.Where(c => c.UserId == userId && c.IsDefault).ToListAsync();
            foreach (var card in cards) { card.IsDefault = false; }

            // Set new default
            var newDefault = await _db.FindAsync(cardId);
            if (newDefault != null) newDefault.IsDefault = true;
        }
    }

    // ─── Beneficiary Repository ───────────────────────────────
    public class BeneficiaryRepository : Repository<FinWalletPro_Core.Models.Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<FinWalletPro_Core.Models.Beneficiary>> GetActiveByUserIdAsync(Guid userId)
            => await _db.Where(b => b.UserId == userId && b.IsActive).ToListAsync();

        public async Task<FinWalletPro_Core.Models.Beneficiary> GetByWalletNumberAsync(Guid userId, string walletNumber)
            => await _db.FirstOrDefaultAsync(b => b.UserId == userId && b.WalletNumber == walletNumber && b.IsActive);
    }
}
