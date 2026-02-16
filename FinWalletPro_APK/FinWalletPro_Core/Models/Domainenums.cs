namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Domainenums
    {
        public enum WalletStatus
        {
            Active = 1,
            Suspended = 2,
            Blocked = 3,
            Closed = 4,
        }

        public enum  TransactionType
        {
            Deposit = 1,
            Withdrawal = 2,
            Transfer = 3,
            Payment = 4,
            Refund = 5,
            Fee = 6,
        }

        public enum TransactionStatus
        {
            Pending = 1,
            Processing = 2,
            Completed = 3,
            Failed = 4,
            Cancelled = 5,
            Reversed = 6,
        }

        public enum CardType
        {
            Visa = 1,
            Mastercard = 2,
            AmericanExpress = 3,
            Discover = 4
        }
    }
}
