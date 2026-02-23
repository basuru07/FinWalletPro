using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        // Constructor 
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Get the authenticated user's account details
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount()
        {
            var accountId = GetCurrentAccountId();
            var account = await _accountService.GetAccountByIdAsync(accountId);
            return Ok(new ApiResponse<AccountDetailDto> { Success = true, Data = MapToDetail(account) });
        }

        // Get current balance
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var accountId = GetCurrentAccountId();
            var account = await _accountService.GetAccountByIdAsync(accountId);
            return Ok(new ApiResponse<BalanceResponseDto>
            {
                Success = true,
                Data = new BalanceResponseDto
                {
                    AccountId = account.AccountId,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    Currency = account.Currency,
                    AsOf = DateTime.UtcNow
                }
            });
        }

        // Update account profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var updated = await _accountService.UpdateAccountAsync(accountId, new Account
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber
            });
            return Ok(new ApiResponse<AccountDetailDto>
            {
                Success = true,
                Message = "Account updated successfully.",
                Data = MapToDetail(updated)
            });
        }

        // ─── Bank Cards ───────────────────────────────────────────────────────

        // Link a new bank card to the account
        [HttpPost("cards")]
        public async Task<IActionResult> AddBankCard([FromBody] AddBankCardRequestDto dto)
        {
            var accountId = GetCurrentAccountId();

            // Mask and hash the card number
            var last4 = dto.CardNumber.Replace(" ", "").Substring(dto.CardNumber.Replace(" ", "").Length - 4);
            var maskedNumber = $"**** **** **** {last4}";
            var cardHash = HashCardNumber(dto.CardNumber.Replace(" ", ""));

            var card = new BankCard
            {
                AccountId = accountId,
                CardHolderName = dto.CardHolderName,
                CardNumberMasked = maskedNumber,
                CardNumberHash = cardHash,
                CardType = dto.CardType,
                CardCategory = dto.CardCategory,
                ExpiryMonth = dto.ExpiryMonth,
                ExpiryYear = dto.ExpiryYear,
                BankName = dto.BankName
            };

            var created = await _accountService.AddBankCardAsync(card);
            return StatusCode(201, new ApiResponse<BankCardDto>
            {
                Success = true,
                Message = "Card linked successfully.",
                Data = MapToCardDto(created)
            });
        }

        // Get all linked bank cards
        [HttpGet("cards")]
        public async Task<IActionResult> GetBankCards()
        {
            var accountId = GetCurrentAccountId();
            var cards = await _accountService.GetBankCardsAsync(accountId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = cards.Select(MapToCardDto)
            });
        }

        // Remove a linked bank card
        [HttpDelete("cards/{cardId}")]
        public async Task<IActionResult> RemoveBankCard(long cardId)
        {
            var accountId = GetCurrentAccountId();
            var result = await _accountService.RemoveBankCardAsync(accountId, cardId);
            if (!result) return NotFound(new ApiResponse<object> { Success = false, Message = "Card not found." });
            return Ok(new ApiResponse<object> { Success = true, Message = "Card removed successfully." });
        }

        // Set a card as the default payment method
        [HttpPatch("cards/{cardId}/set-default")]
        public async Task<IActionResult> SetDefaultCard(long cardId)
        {
            var accountId = GetCurrentAccountId();
            await _accountService.SetDefaultCardAsync(accountId, cardId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Default card updated." });
        }

        // ─── Helpers ──────────────────────────────────────────────────────────
        private long GetCurrentAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("Invalid token.");
            return long.Parse(claim.Value);
        }

        private static string HashCardNumber(string cardNumber)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(cardNumber)));
        }

        private static AccountDetailDto MapToDetail(Account a) => new()
        {
            AccountId = a.AccountId,
            FullName = a.FullName,
            Email = a.Email,
            PhoneNumber = a.PhoneNumber,
            AccountNumber = a.AccountNumber,
            Balance = a.Balance,
            Currency = a.Currency,
            AccountStatus = a.AccountStatus,
            CreatedAt = a.CreatedAt,
            BankCards = a.BankCards?.Select(MapToCardDto).ToList()
        };

        private static BankCardDto MapToCardDto(BankCard c) => new()
        {
            CardId = c.CardId,
            CardHolderName = c.CardHolderName,
            CardNumberMasked = c.CardNumberMasked,
            CardType = c.CardType,
            CardCategory = c.CardCategory,
            ExpiryMonth = c.ExpiryMonth,
            ExpiryYear = c.ExpiryYear,
            BankName = c.BankName,
            IsDefault = c.IsDefault,
            LinkedAt = c.LinkedAt
        };
    }
}
