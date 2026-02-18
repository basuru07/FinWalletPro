using FinWalletPro_APK.FinWalletPro_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBeneficiaryService _beneficiaryService;

        public UserController(IUserService userService, IBeneficiaryService beneficiaryService)
        {
            _userService = userService;
            _beneficiaryService = beneficiaryService;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // GET api/user/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userService.GetProfileAsync(CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // PUT api/user/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.UpdateProfileAsync(CurrentUserId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/user/beneficiaries
        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetBeneficiaries()
        {
            var result = await _beneficiaryService.GetBeneficiariesAsync(CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/user/beneficiaries
        [HttpPost("beneficiaries")]
        public async Task<IActionResult> AddBeneficiary([FromBody] AddBeneficiaryCommand request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var dto = new AddBeneficiaryDto
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                WalletNumber = request.WalletNumber,
                Nickname = request.Nickname
            };

            var result = await _beneficiaryService.AddBeneficiaryAsync(CurrentUserId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE api/user/beneficiaries/{id}
        [HttpDelete("beneficiaries/{id}")]
        public async Task<IActionResult> DeleteBeneficiary(Guid id)
        {
            var result = await _beneficiaryService.DeleteBeneficiaryAsync(CurrentUserId, id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankCardController : ControllerBase
    {
        private readonly IBankCardService _bankCardService;

        public BankCardController(IBankCardService bankCardService)
        {
            _bankCardService = bankCardService;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // GET api/bankcard
        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var result = await _bankCardService.GetCardsAsync(CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/bankcard
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] AddBankCardRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var dto = new AddBankCardDto
            {
                CardHolderName = request.CardHolderName,
                CardNumber = request.CardNumber,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                CVV = request.CVV,
                SetAsDefault = request.SetAsDefault
            };

            var result = await _bankCardService.AddCardAsync(CurrentUserId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE api/bankcard/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            var result = await _bankCardService.DeleteCardAsync(CurrentUserId, id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
