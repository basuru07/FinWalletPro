using System.Transactions;
using static FinWalletPro_APK.FinWalletPro_Core.Models.Domainenums;

namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string Currency { get; set; }
        public WalletStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> OutgoingTransactions { get; set; }
        public virtual ICollection<Transaction> IncomingTransaction { get; set; }

        // Wallet Constructor
        public Wallet()
        {
            Id = Guid.NewGuid();
            CreatedAt  = DateTime.UtcNow;
            Currency = "USD";
            Status = WalletStatus.Active;
            Balance = 0;
            AvailableBalance = 0;
            OutgoingTransactions = new HashSet<Transaction>();
            IncomingTransaction = new HashSet<Transaction>();

        }

        // Credit -> add money to wallet
        public void Credit(decimal amount)

        {
            Balance += amount;
            AvailableBalance += amount;
            UpdateAt = DateTime.UtcNow;
        }

        // Can Debit -> check if wallet can withdraw money
        public bool CanDebit(decimal amount)
        {
            return AvailableBalance >= amount && Status == WalletStatus.Active;
        }

        // Debit -> remove money 
        public void Debit(decimal amount)
        {
            if (!CanDebit(amount))
                throw new InvalidOperationException("Insufficient balance or wallet is inactive");

            Balance -= amount;
            AvailableBalance -= amount;
            UpdateAt = DateTime.UtcNow;
        }
    }
}
