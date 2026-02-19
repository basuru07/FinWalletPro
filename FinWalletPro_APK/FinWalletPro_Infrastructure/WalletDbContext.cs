using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BankCard> BankCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("WalletAccounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<BankCard>().ToTable("BankCards");
        }
    }
}
