using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification> GetByIdAsync(long notificationId);
        Task<IEnumerable<Notification>> GetByAccountIdAsync(long accountId, bool unreadOnly);
        Task<Notification> UpdateAsync(Notification notification);
        Task<bool> DeleteAsync(long notificationId);
        Task MarkAllReadAsync(long accountId);
        Task<int> GetUnreadCountAsync(long accountId);
    }
}
