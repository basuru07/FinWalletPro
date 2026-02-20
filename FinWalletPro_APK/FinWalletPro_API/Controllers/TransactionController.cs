using Braintree;
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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>Transfer money to another wallet</summary>
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var tx = await _transactionService.TransferAsync(
                accountId, dto.ReceiverAccountNumber, dto.Amount, dto.Description, dto.Category);

            return StatusCode(201, new ApiResponse<TransactionResponseDto>
            {
                Success = true,
                Message = "Transfer completed successfully.",
                Data = MapToResponse(tx)
            });
        }

        /// <summary>Deposit money into wallet</summary>
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var tx = await _transactionService.DepositAsync(accountId, dto.Amount, dto.Description);

            return StatusCode(201, new ApiResponse<TransactionResponseDto>
            {
                Success = true,
                Message = "Deposit successful.",
                Data = MapToResponse(tx)
            });
        }

        /// <summary>Withdraw money from wallet</summary>
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var tx = await _transactionService.WithdrawAsync(accountId, dto.Amount, dto.Description);

            return StatusCode(201, new ApiResponse<TransactionResponseDto>
            {
                Success = true,
                Message = "Withdrawal successful.",
                Data = MapToResponse(tx)
            });
        }

        /// <summary>Get transaction history with filtering and pagination</summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] TransactionFilterRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var filter = new TransactionFilter
            {
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                TransactionType = dto.TransactionType,
                Status = dto.Status,
                MinAmount = dto.MinAmount,
                MaxAmount = dto.MaxAmount,
                Category = dto.Category,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize
            };

            var transactions = await _transactionService.GetTransactionHistoryAsync(accountId, filter);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    page = dto.PageNumber,
                    pageSize = dto.PageSize,
                    transactions = transactions.Select(MapToResponse)
                }
            });
        }

        /// <summary>Get account statement for a date range</summary>
        [HttpGet("statement")]
        public async Task<IActionResult> GetStatement([FromQuery] StatementRequestDto dto)
        {
            var accountId = GetCurrentAccountId();
            var transactions = await _transactionService.GetStatementAsync(accountId, dto.FromDate, dto.ToDate);
            var txList = transactions.ToList();

            var totalSent = await _transactionService.GetTotalSentAsync(accountId, dto.FromDate, dto.ToDate);
            var totalReceived = await _transactionService.GetTotalReceivedAsync(accountId, dto.FromDate, dto.ToDate);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    summary = new
                    {
                        totalTransactions = txList.Count,
                        totalSent,
                        totalReceived,
                        fromDate = dto.FromDate,
                        toDate = dto.ToDate
                    },
                    transactions = txList.Select(MapToResponse)
                }
            });
        }

        /// <summary>Get a single transaction by ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var tx = await _transactionService.GetTransactionByIdAsync(id);
            return Ok(new ApiResponse<TransactionResponseDto> { Success = true, Data = MapToResponse(tx) });
        }

        /// <summary>Get a single transaction by reference number</summary>
        [HttpGet("reference/{reference}")]
        public async Task<IActionResult> GetByReference(string reference)
        {
            var tx = await _transactionService.GetTransactionByReferenceAsync(reference);
            return Ok(new ApiResponse<TransactionResponseDto> { Success = true, Data = MapToResponse(tx) });
        }

        /// <summary>Reverse a completed transfer transaction</summary>
        [HttpPost("{id}/reverse")]
        public async Task<IActionResult> ReverseTransaction(long id, [FromBody] ReverseTransactionRequestDto dto)
        {
            await _transactionService.ReverseTransactionAsync(id, dto.Reason);
            return Ok(new ApiResponse<object> { Success = true, Message = "Transaction reversed successfully." });
        }

        // ─── Helpers ──────────────────────────────────────────────────────────
        private long GetCurrentAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("Invalid token.");
            return long.Parse(claim.Value);
        }

        private static TransactionResponseDto MapToResponse(Transaction t) => new()
        {
            TransactionId = t.TransactionId,
            TransactionReference = t.TransactionReference,
            SenderName = t.SenderAccount?.FullName,
            SenderAccountNumber = t.SenderAccount?.AccountNumber,
            ReceiverName = t.ReceiverAccount?.FullName,
            ReceiverAccountNumber = t.ReceiverAccount?.AccountNumber,
            Amount = t.Amount,
            Fee = t.Fee,
            TotalAmount = t.Amount + t.Fee,
            Currency = t.Currency,
            TransactionType = t.TransactionType,
            Status = t.Status,
            Description = t.Description,
            Category = t.Category,
            BalanceAfter = t.BalanceAfter,
            TransactionDate = t.TransactionDate
        };
    }
}
