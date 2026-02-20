using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsAsync(long accountId, bool unreadOnly = false);
        Task<bool> MarkAsReadAsync(long notificationId, long accountId);
        Task<bool> MarkAllAsReadAsync(long accountId);
        Task<int> GetUnreadCountAsync(long accountId);
        Task<bool> DeleteNotificationAsync(long notificationId, long accountId);
        Task SendTransactionNotificationAsync(long accountId, string title, string message, string referenceId);
        Task SendSecurityNotificationAsync(long accountId, string title, string message);
    }
}
