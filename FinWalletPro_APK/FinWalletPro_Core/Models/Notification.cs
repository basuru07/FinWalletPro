namespace FinWalletPro_APK.FinWalletPro_Core.Models
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public long AccountId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? NotificationType { get; set; } // Transaction, Security, Promotional, System
        public bool IsRead { get; set; } = false;
        public string? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Navigate property to Account
        public Account? Account { get; set; }
    }
}
