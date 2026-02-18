using System;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public IRepository<FinWalletPro_Core.Models.User> Users { get; }
        public IRepository<FinWalletPro_Core.Models.Wallet> Wallets { get; }
        public IRepository<FinWalletPro_Core.Models.Transaction> Transactions { get; }
        public IRepository<FinWalletPro_Core.Models.BankCard> BankCards { get; }
        public IRepository<FinWalletPro_Core.Models.Beneficiary> Beneficiaries { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            Wallets = new WalletRepository(_context);
            Transactions = new TransactionRepository(_context);
            BankCards = new BankCardRepository(_context);
            Beneficiaries = new BeneficiaryRepository(_context);
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
