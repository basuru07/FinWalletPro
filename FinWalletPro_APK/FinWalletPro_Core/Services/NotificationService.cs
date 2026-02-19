using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _repo;

        public NotificationService(NotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateNotificationAsync(int accountId, string message)
        {
            var notification = new Notification
            {
                AccountId = accountId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _repo.AddAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int accountId)
        {
            return await _repo.GetByAccountIdAsync(accountId);
        }
    }
}
