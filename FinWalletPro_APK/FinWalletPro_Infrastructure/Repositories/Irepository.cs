using System.Linq.Expressions;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface Irepository<T> where T : class
    {
        Task<T> GetIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IWalletRepository Wallets { get; }
        ITransactionRepository Transactions { get; }
        IBankCardRepository BankCards { get; }
        IBeneficiaryRepository Beneficiaries { get; }

        Task<int> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}
