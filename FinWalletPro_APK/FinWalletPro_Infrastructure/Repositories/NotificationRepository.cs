using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly WalletDbContext _context;

        public NotificationRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> GetByIdAsync(long notificationId)
            => await _context.Notifications.FindAsync(notificationId);

        public async Task<IEnumerable<Notification>> GetByAccountIdAsync(long accountId, bool unreadOnly)
        {
            var query = _context.Notifications.Where(n => n.AccountId == accountId);
            if (unreadOnly)
                query = query.Where(n => !n.IsRead);
            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(long notificationId)
        {
            var n = await _context.Notifications.FindAsync(notificationId);
            if (n == null) return false;
            _context.Notifications.Remove(n);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task MarkAllReadAsync(long accountId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.AccountId == accountId && !n.IsRead)
                .ToListAsync();

            foreach (var n in notifications)
            {
                n.IsRead = true;
                n.ReadAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(long accountId)
            => await _context.Notifications
                .CountAsync(n => n.AccountId == accountId && !n.IsRead);
    }
}
