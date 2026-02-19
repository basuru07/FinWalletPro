using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpPost("send")]
        public async Task<IActionResult> Send(int accountId, string message)
        {
            await _notificationService.NotifyAsync(accountId, message);
            return Ok(new { Success = true });
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(accountId);
            return Ok(notifications);
        }
    }
}
