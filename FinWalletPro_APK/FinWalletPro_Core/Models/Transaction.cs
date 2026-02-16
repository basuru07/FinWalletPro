using System.Transactions;

namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string TransactionReference { get; set; } // for unique trasaction ID
        public Guid? SourceWalletId { get; set; }
        public Guid? DestinationWaletId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string Description { get; set; }
        public string? FailureReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Additional Metadata
        public string? BeneficiaryName { get; set; }
        public string? BankCardLast4 { get; set; }

        // Navigation Properties
        public virtual Wallet SourceWallet { get; set; }
        public virtual Wallet DestinationWallet { get; set;}

        // Transaction
        public Transaction()
        {
            Id = Guid.NewGuid();
            TransactionReference = GenerateReference();
            CreatedAt = DateTime.UtcNow;
            Status = TransactionStatus.Pending;
            Currency = "USD";
        }

        // Generate Reference
        private string GenerateReference()
        {
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        // Mark As a Completed
        private void MarkAsCompleted()
        {
            Status = TransactionStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Mark as a Failed
       public void MarkAsFailed(string reason)
        {
            Status = TransactionStatus.Failed;
            FailureReason = reason;
            UpdatedAt = DateTime.UtcNow;

        }

    }
}
