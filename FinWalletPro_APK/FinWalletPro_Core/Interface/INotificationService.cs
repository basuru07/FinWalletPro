using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public class INotificationService
    {
        Task NotifyAsync(int accountId, string message);
        Task<List<Notification>> GetNotificationsAsync(int accountId);
    }
}
