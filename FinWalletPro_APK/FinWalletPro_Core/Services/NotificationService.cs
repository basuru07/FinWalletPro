using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            return await _notificationRepository.CreateAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(long accountId, bool unreadOnly = false)
            => await _notificationRepository.GetByAccountIdAsync(accountId, unreadOnly);

        public async Task<bool> MarkAsReadAsync(long notificationId, long accountId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null) throw new KeyNotFoundException("Notification not found.");
            if (notification.AccountId != accountId)
                throw new UnauthorizedAccessException("Access denied.");

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(long accountId)
        {
            await _notificationRepository.MarkAllReadAsync(accountId);
            return true;
        }

        public async Task<int> GetUnreadCountAsync(long accountId)
            => await _notificationRepository.GetUnreadCountAsync(accountId);

        public async Task<bool> DeleteNotificationAsync(long notificationId, long accountId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null) throw new KeyNotFoundException("Notification not found.");
            if (notification.AccountId != accountId)
                throw new UnauthorizedAccessException("Access denied.");

            return await _notificationRepository.DeleteAsync(notificationId);
        }

        public async Task SendTransactionNotificationAsync(long accountId, string title, string message, string referenceId)
        {
            await CreateNotificationAsync(new Notification
            {
                AccountId = accountId,
                Title = title,
                Message = message,
                NotificationType = "Transaction",
                ReferenceId = referenceId,
                ReferenceType = "Transaction",
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task SendSecurityNotificationAsync(long accountId, string title, string message)
        {
            await CreateNotificationAsync(new Notification
            {
                AccountId = accountId,
                Title = title,
                Message = message,
                NotificationType = "Security",
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
