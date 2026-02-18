using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            // ── USERS ─────────────────────────────────────────
            model.Entity<User>(e =>
            {
                e.ToTable("USERS");
                e.HasKey(x => x.Id);
                e.Property(x => x.Email).IsRequired().HasMaxLength(100);
                e.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
                e.Property(x => x.LastName).IsRequired().HasMaxLength(50);
                e.Property(x => x.PhoneNumber).HasMaxLength(20);
                e.Property(x => x.PasswordHash).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();

                // User → Wallet (1-to-1)
                e.HasOne(x => x.Wallet)
                 .WithOne(w => w.User)
                 .HasForeignKey<Wallet>(w => w.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── WALLETS ───────────────────────────────────────
            model.Entity<Wallet>(e =>
            {
                e.ToTable("WALLETS");
                e.HasKey(x => x.Id);
                e.Property(x => x.WalletNumber).IsRequired().HasMaxLength(20);
                e.Property(x => x.Balance).HasColumnType("decimal(18,2)");
                e.Property(x => x.AvailableBalance).HasColumnType("decimal(18,2)");
                e.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
                e.HasIndex(x => x.WalletNumber).IsUnique();
            });

            // ── TRANSACTIONS ──────────────────────────────────
            model.Entity<Transaction>(e =>
            {
                e.ToTable("TRANSACTIONS");
                e.HasKey(x => x.Id);
                e.Property(x => x.TransactionReference).IsRequired().HasMaxLength(50);
                e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                e.Property(x => x.Fee).HasColumnType("decimal(18,2)");
                e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.Currency).HasMaxLength(3);
                e.Property(x => x.Description).HasMaxLength(500);
                e.HasIndex(x => x.TransactionReference).IsUnique();
                e.HasIndex(x => x.CreatedAt);

                // Transaction → Source Wallet (many-to-one)
                e.HasOne(x => x.SourceWallet)
                 .WithMany(w => w.OutgoingTransactions)
                 .HasForeignKey(x => x.SourceWalletId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Transaction → Destination Wallet (many-to-one)
                e.HasOne(x => x.DestinationWallet)
                 .WithMany(w => w.IncomingTransaction)
                 .HasForeignKey(x => x.DestinationWalletId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── BANK CARDS ────────────────────────────────────
            model.Entity<BankCard>(e =>
            {
                e.ToTable("BANK_CARDS");
                e.HasKey(x => x.Id);
                e.Property(x => x.CardHolderName).IsRequired().HasMaxLength(100);
                e.Property(x => x.CardNumber).IsRequired().HasMaxLength(500); // Encrypted
                e.Property(x => x.Last4Digits).HasMaxLength(4);
                e.Property(x => x.CardType).HasMaxLength(20);

                // BankCard → User (many-to-one)
                e.HasOne(x => x.User)
                 .WithMany(u => u.BankCards)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── BENEFICIARIES ─────────────────────────────────
            model.Entity<Beneficiary>(e =>
            {
                e.ToTable("BENEFICIARIES");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(100);
                e.Property(x => x.WalletNumber).IsRequired().HasMaxLength(20);
                e.Property(x => x.Nickname).HasMaxLength(50);

                // Beneficiary → User (many-to-one)
                e.HasOne(x => x.User)
                 .WithMany(u => u.Beneficiaries)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
