using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Intercom.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // POST api/transaction/transfer
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferCommand request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var command = new Models.TransferCommand
            {
                UserId = CurrentUserId,
                DestinationWalletNumber = request.DestinationWalletNumber,
                Amount = request.Amount,
                Description = request.Description,
                Pin = request.Pin
            };

            var result = await _transactionService.TransferAsync(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/transaction/deposit
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositCommand request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var command = new Models.DepositCommand
            {
                UserId = CurrentUserId,
                BankCardId = request.BankCardId,
                Amount = request.Amount,
                Description = request.Description
            };

            var result = await _transactionService.DepositAsync(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/transaction/withdraw
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var command = new Models.WithdrawCommand
            {
                UserId = CurrentUserId,
                BankCardId = request.BankCardId,
                Amount = request.Amount,
                Description = request.Description,
                Pin = request.Pin
            };

            var result = await _transactionService.WithdrawAsync(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/transaction/history
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] TransactionFilterDto filter)
        {
            var txnFilter = new TransactionFilter
            {
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                Type = filter.Type,
                Status = filter.Status,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            var result = await _transactionService.GetHistoryAsync(CurrentUserId, txnFilter);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/transaction/{reference}
        [HttpGet("{reference}")]
        public async Task<IActionResult> GetByReference(string reference)
        {
            var result = await _transactionService.GetByReferenceAsync(reference);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
