using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) => _accountService = accountService;

        [HttpGet("{accountId}/balance")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var balance = await _accountService.GetBalanceAsync(accountId);
            return Ok(new { AccountId = accountId, Balance = balance });
        }
    }
}
