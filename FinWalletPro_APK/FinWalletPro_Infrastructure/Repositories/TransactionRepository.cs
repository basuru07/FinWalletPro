using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletDbContext _context;

        public TransactionRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> GetByIdAsync(long transactionId)
            => await _context.Transactions
                .Include(t => t.SenderAccount)
                .Include(t => t.ReceiverAccount)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

        public async Task<Transaction> GetByReferenceAsync(string reference)
            => await _context.Transactions
                .Include(t => t.SenderAccount)
                .Include(t => t.ReceiverAccount)
                .FirstOrDefaultAsync(t => t.TransactionReference == reference);

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(long accountId, TransactionFilter filter)
        {
            var query = _context.Transactions
                .Include(t => t.SenderAccount)
                .Include(t => t.ReceiverAccount)
                .Where(t => t.SenderAccountId == accountId || t.ReceiverAccountId == accountId);

            if (filter.FromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(t => t.TransactionDate <= filter.ToDate.Value);

            if (!string.IsNullOrEmpty(filter.TransactionType))
                query = query.Where(t => t.TransactionType == filter.TransactionType);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(t => t.Status == filter.Status);

            if (filter.MinAmount.HasValue)
                query = query.Where(t => t.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(t => t.Amount <= filter.MaxAmount.Value);

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(t => t.Category == filter.Category);

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetStatementAsync(long accountId, DateTime from, DateTime to)
            => await _context.Transactions
                .Include(t => t.SenderAccount)
                .Include(t => t.ReceiverAccount)
                .Where(t => (t.SenderAccountId == accountId || t.ReceiverAccountId == accountId)
                    && t.TransactionDate >= from
                    && t.TransactionDate <= to
                    && t.Status == "Completed")
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

        public async Task<decimal> GetTotalSentAsync(long accountId, DateTime from, DateTime to)
            => await _context.Transactions
                .Where(t => t.SenderAccountId == accountId
                    && t.TransactionType == "Transfer"
                    && t.Status == "Completed"
                    && t.TransactionDate >= from
                    && t.TransactionDate <= to)
                .SumAsync(t => t.Amount);

        public async Task<decimal> GetTotalReceivedAsync(long accountId, DateTime from, DateTime to)
            => await _context.Transactions
                .Where(t => t.ReceiverAccountId == accountId
                    && t.TransactionType == "Transfer"
                    && t.Status == "Completed"
                    && t.TransactionDate >= from
                    && t.TransactionDate <= to)
                .SumAsync(t => t.Amount);

        
    }
}
