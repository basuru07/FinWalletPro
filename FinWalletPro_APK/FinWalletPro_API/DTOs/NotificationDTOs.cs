namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
