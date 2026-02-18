using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Read
        Task<T> GetIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        
        // Write
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(T entity);
    }

    public interface IUnitOfWork : IDisposable
    {
        IRepository<FinWalletPro_Core.Models.User> Users { get; }
        IRepository<FinWalletPro_Core.Models.Wallet> Wallets { get; }
        IRepository<FinWalletPro_Core.Models.Transaction> Transactions { get; }
        IRepository<FinWalletPro_Core.Models.BankCard> BankCards { get; }
        IRepository<FinWalletPro_Core.Models.Beneficiary> Beneficiary { get; }

        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
