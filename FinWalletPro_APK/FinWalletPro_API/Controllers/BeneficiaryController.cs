using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        public BeneficiaryController(IBeneficiaryService beneficiaryService) => _beneficiaryService = beneficiaryService;

        [HttpPost("add")]
        public async Task<IActionResult> Add(BeneficiaryRequest request)
        {
            var beneficiary = new Beneficiary
            {
                AccountId = request.AccountId,
                Name = request.Name,
                WalletAddress = request.WalletAddress
            };
            var result = await _beneficiaryService.AddBeneficiaryAsync(beneficiary);
            return Ok(result);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAll(int accountId)
        {
            var beneficiaries = await _beneficiaryService.GetBeneficiariesAsync(accountId);
            return Ok(beneficiaries);
        }
    }
}
