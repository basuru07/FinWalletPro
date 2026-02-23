namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Notification_ResponseDto
    public class NotificationResponseDto
    {
        public long NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? NotificationType { get; set; }
        public bool IsRead { get; set; }
        public string? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    // Notification_CountDto
    public class NotificationCountDto
    {
        public int UnreadCount { get; set; }
    }
}
