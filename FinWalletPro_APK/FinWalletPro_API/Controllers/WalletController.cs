using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService) 
        {
            _walletService = walletService;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var result = await _walletService.GetWalletAsync(CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var result = await _walletService.GetBalanceAsync(CurrentUserId);
            return result.Success ? Ok(result): BadRequest(result);
        }
    }
}
