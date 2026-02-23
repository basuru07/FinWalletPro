using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get all notifications (optionally filter unread only)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool unreadOnly = false)
        {
            var accountId = GetCurrentAccountId();
            var notifications = await _notificationService.GetNotificationsAsync(accountId, unreadOnly);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = notifications.Select(n => new NotificationResponseDto
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    IsRead = n.IsRead,
                    ReferenceId = n.ReferenceId,
                    ReferenceType = n.ReferenceType,
                    CreatedAt = n.CreatedAt,
                    ReadAt = n.ReadAt
                })
            });
        }

        // Get count of unread notifications
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var accountId = GetCurrentAccountId();
            var count = await _notificationService.GetUnreadCountAsync(accountId);
            return Ok(new ApiResponse<NotificationCountDto>
            {
                Success = true,
                Data = new NotificationCountDto { UnreadCount = count }
            });
        }

        // Mark a notification as read
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var accountId = GetCurrentAccountId();
            await _notificationService.MarkAsReadAsync(id, accountId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Notification marked as read." });
        }

        // Mark all notifications as read
        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var accountId = GetCurrentAccountId();
            await _notificationService.MarkAllAsReadAsync(accountId);
            return Ok(new ApiResponse<object> { Success = true, Message = "All notifications marked as read." });
        }

        // Delete a notification
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var accountId = GetCurrentAccountId();
            await _notificationService.DeleteNotificationAsync(id, accountId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Notification deleted." });
        }

        // ─── Helpers ──────────────────────────────────────────────────────────
        private long GetCurrentAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("Invalid token.");
            return long.Parse(claim.Value);
        }
    }
}
