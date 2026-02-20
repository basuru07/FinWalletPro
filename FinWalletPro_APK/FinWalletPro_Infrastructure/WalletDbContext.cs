using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

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
            base.OnModelCreating(modelBuilder);

            // ─── Account ─────────────────────────────────────────────────────
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("ACCOUNTS");
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId)
                      .HasColumnName("ACCOUNT_ID")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("Oracle:ValueGenerationStrategy",
                                     Oracle.EntityFrameworkCore.ValueGeneration.OracleValueGenerationStrategy.SequenceTrigger);

                entity.Property(e => e.UserId).HasColumnName("USER_ID").HasMaxLength(50).IsRequired();
                entity.Property(e => e.FullName).HasColumnName("FULL_NAME").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH").HasMaxLength(500).IsRequired();
                entity.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(20);
                entity.Property(e => e.Balance).HasColumnName("BALANCE").HasColumnType("NUMBER(18,2)").HasDefaultValue(0);
                entity.Property(e => e.Currency).HasColumnName("CURRENCY").HasMaxLength(10).HasDefaultValue("USD");
                entity.Property(e => e.AccountStatus).HasColumnName("ACCOUNT_STATUS").HasMaxLength(20).HasDefaultValue("Active");
                entity.Property(e => e.AccountNumber).HasColumnName("ACCOUNT_NUMBER").HasMaxLength(30).IsRequired();
                entity.HasIndex(e => e.AccountNumber).IsUnique();
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT");
                entity.Property(e => e.RefreshToken).HasColumnName("REFRESH_TOKEN").HasMaxLength(500);
                entity.Property(e => e.RefreshTokenExpiry).HasColumnName("REFRESH_TOKEN_EXPIRY");
            });

            // ─── Transaction ──────────────────────────────────────────────────
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("TRANSACTIONS");
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                      .HasColumnName("TRANSACTION_ID")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("Oracle:ValueGenerationStrategy",
                                     Oracle.EntityFrameworkCore.ValueGeneration.OracleValueGenerationStrategy.SequenceTrigger);

                entity.Property(e => e.TransactionReference).HasColumnName("TRANSACTION_REFERENCE").HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.TransactionReference).IsUnique();
                entity.Property(e => e.SenderAccountId).HasColumnName("SENDER_ACCOUNT_ID");
                entity.Property(e => e.ReceiverAccountId).HasColumnName("RECEIVER_ACCOUNT_ID");
                entity.Property(e => e.Amount).HasColumnName("AMOUNT").HasColumnType("NUMBER(18,2)");
                entity.Property(e => e.Currency).HasColumnName("CURRENCY").HasMaxLength(10);
                entity.Property(e => e.TransactionType).HasColumnName("TRANSACTION_TYPE").HasMaxLength(30);
                entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
                entity.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(500);
                entity.Property(e => e.Category).HasColumnName("CATEGORY").HasMaxLength(100);
                entity.Property(e => e.Fee).HasColumnName("FEE").HasColumnType("NUMBER(18,2)").HasDefaultValue(0);
                entity.Property(e => e.BalanceAfter).HasColumnName("BALANCE_AFTER").HasColumnType("NUMBER(18,2)");
                entity.Property(e => e.TransactionDate).HasColumnName("TRANSACTION_DATE");
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
                

                entity.HasOne(e => e.SenderAccount)
                      .WithMany(a => a.SentTransactions)
                      .HasForeignKey(e => e.SenderAccountId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReceiverAccount)
                      .WithMany(a => a.ReceivedTransactions)
                      .HasForeignKey(e => e.ReceiverAccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Beneficiary ──────────────────────────────────────────────────
            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.ToTable("BENEFICIARIES");
                entity.HasKey(e => e.BeneficiaryId);
                entity.Property(e => e.BeneficiaryId)
                      .HasColumnName("BENEFICIARY_ID")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("Oracle:ValueGenerationStrategy",
                                     Oracle.EntityFrameworkCore.ValueGeneration.OracleValueGenerationStrategy.SequenceTrigger);

                entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
                entity.Property(e => e.NickName).HasColumnName("NICK_NAME").HasMaxLength(100);
                entity.Property(e => e.BeneficiaryAccountNumber).HasColumnName("BENEFICIARY_ACCOUNT_NUMBER").HasMaxLength(50).IsRequired();
                entity.Property(e => e.BeneficiaryName).HasColumnName("BENEFICIARY_NAME").HasMaxLength(200).IsRequired();
                entity.Property(e => e.BeneficiaryEmail).HasColumnName("BENEFICIARY_EMAIL").HasMaxLength(200);
                entity.Property(e => e.BeneficiaryPhone).HasColumnName("BENEFICIARY_PHONE").HasMaxLength(20);
                entity.Property(e => e.BankName).HasColumnName("BANK_NAME").HasMaxLength(100);
                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").HasColumnType("NUMBER(1)");
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT");

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.Beneficiaries)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Notification ─────────────────────────────────────────────────
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("NOTIFICATIONS");
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.NotificationId)
                      .HasColumnName("NOTIFICATION_ID")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("Oracle:ValueGenerationStrategy",
                                     Oracle.EntityFrameworkCore.ValueGeneration.OracleValueGenerationStrategy.SequenceTrigger);

                entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
                entity.Property(e => e.Title).HasColumnName("TITLE").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Message).HasColumnName("MESSAGE").HasMaxLength(2000).IsRequired();
                entity.Property(e => e.NotificationType).HasColumnName("NOTIFICATION_TYPE").HasMaxLength(30);
                entity.Property(e => e.IsRead).HasColumnName("IS_READ").HasColumnType("NUMBER(1)").HasDefaultValue(0);
                entity.Property(e => e.ReferenceId).HasColumnName("REFERENCE_ID").HasMaxLength(100);
                entity.Property(e => e.ReferenceType).HasColumnName("REFERENCE_TYPE").HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
                entity.Property(e => e.ReadAt).HasColumnName("READ_AT");

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.Notifications)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── BankCard ─────────────────────────────────────────────────────
            modelBuilder.Entity<BankCard>(entity =>
            {
                entity.ToTable("BANK_CARDS");
                entity.HasKey(e => e.CardId);
                entity.Property(e => e.CardId)
                      .HasColumnName("CARD_ID")
                      .ValueGeneratedOnAdd()
                      .HasAnnotation("Oracle:ValueGenerationStrategy",
                                     Oracle.EntityFrameworkCore.ValueGeneration.OracleValueGenerationStrategy.SequenceTrigger);

                entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
                entity.Property(e => e.CardHolderName).HasColumnName("CARD_HOLDER_NAME").HasMaxLength(200).IsRequired();
                entity.Property(e => e.CardNumberMasked).HasColumnName("CARD_NUMBER_MASKED").HasMaxLength(20);
                entity.Property(e => e.CardNumberHash).HasColumnName("CARD_NUMBER_HASH").HasMaxLength(500);
                entity.Property(e => e.CardType).HasColumnName("CARD_TYPE").HasMaxLength(20);
                entity.Property(e => e.CardCategory).HasColumnName("CARD_CATEGORY").HasMaxLength(20);
                entity.Property(e => e.ExpiryMonth).HasColumnName("EXPIRY_MONTH").HasMaxLength(2);
                entity.Property(e => e.ExpiryYear).HasColumnName("EXPIRY_YEAR").HasMaxLength(4);
                entity.Property(e => e.BankName).HasColumnName("BANK_NAME").HasMaxLength(100);
                entity.Property(e => e.IsDefault).HasColumnName("IS_DEFAULT").HasColumnType("NUMBER(1)");
                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").HasColumnType("NUMBER(1)");
                entity.Property(e => e.LinkedAt).HasColumnName("LINKED_AT");
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT");

                entity.HasOne(e => e.Account)
                      .WithMany(a => a.BankCards)
                      .HasForeignKey(e => e.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
