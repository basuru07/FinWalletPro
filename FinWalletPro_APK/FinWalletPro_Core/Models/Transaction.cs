using System.ComponentModel.DataAnnotations.Schema;

namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string? TransactionReference { get; set; }
        public long SenderAccountId { get; set; }
        public long ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Currency {  get; set; }
        public string? TransactionType { get; set; } 
        public string Status { get; set; } = "Pending";
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal Fee { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        //[Column("UPDATED_AT")]
        //public DateTime? UpdateAt {  get; set; }

        //Navigate property to SENDER Account | Create relationship (Many Transactions → One Account) 
        public Account? SenderAccount { get; set; }
        public Account? ReceiverAccount { get; set; }    
    }


}
