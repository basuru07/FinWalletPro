using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;

        // Constructor
        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        // Get all beneficiaries for the current account
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accountId = GetCurrentAccountId();
            var beneficiaries = await _beneficiaryService.GetBeneficiariesAsync(accountId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = beneficiaries.Select(MapToDto)
            });
        }

        // Get a specific beneficiary
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var beneficiary = await _beneficiaryService.GetBeneficiaryByIdAsync(id);
            return Ok(new ApiResponse<BeneficiaryResponseDto> { Success = true, Data = MapToDto(beneficiary) });
        }

        // Add a new beneficiary
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddBeneficiaryRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var beneficiary = new Beneficiary
            {
                AccountId = accountId,
                BeneficiaryName = dto.BeneficiaryName,
                BeneficiaryAccountNumber = dto.BeneficiaryAccountNumber,
                NickName = dto.NickName,
                BeneficiaryEmail = dto.BeneficiaryEmail,
                BeneficiaryPhone = dto.BeneficiaryPhone,
                BankName = dto.BankName
            };

            var created = await _beneficiaryService.AddBeneficiaryAsync(beneficiary);
            return StatusCode(201, new ApiResponse<BeneficiaryResponseDto>
            {
                Success = true,
                Message = "Beneficiary added successfully.",
                Data = MapToDto(created)
            });
        }

        // Update beneficiary details
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateBeneficiaryRequestDto dto)
        {
            var updated = await _beneficiaryService.UpdateBeneficiaryAsync(id, new Beneficiary
            {
                NickName = dto.NickName,
                BeneficiaryName = dto.BeneficiaryName,
                BeneficiaryPhone = dto.BeneficiaryPhone,
                BankName = dto.BankName
            });

            return Ok(new ApiResponse<BeneficiaryResponseDto>
            {
                Success = true,
                Message = "Beneficiary updated.",
                Data = MapToDto(updated)
            });
        }

        // Remove a beneficiary
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            var accountId = GetCurrentAccountId();
            await _beneficiaryService.RemoveBeneficiaryAsync(accountId, id);
            return Ok(new ApiResponse<object> { Success = true, Message = "Beneficiary removed." });
        }

        // ─── Helpers ──────────────────────────────────────────────────────────
        private long GetCurrentAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("Invalid token.");
            return long.Parse(claim.Value);
        }

        private static BeneficiaryResponseDto MapToDto(Beneficiary b) => new()
        {
            BeneficiaryId = b.BeneficiaryId,
            BeneficiaryName = b.BeneficiaryName,
            BeneficiaryAccountNumber = b.BeneficiaryAccountNumber,
            NickName = b.NickName,
            BeneficiaryEmail = b.BeneficiaryEmail,
            BeneficiaryPhone = b.BeneficiaryPhone,
            BankName = b.BankName,
            CreatedAt = b.CreatedAt
        };
    }
}
