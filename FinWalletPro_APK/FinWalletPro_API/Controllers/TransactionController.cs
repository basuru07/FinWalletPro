using Braintree;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinWalletPro_APK.FinWalletPro_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService) => _transactionService = transactionService;

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferRequest request)
        {
            var transaction = await _transactionService.TransferAsync(request.FromAccountId, request.ToAccountId, request.Amount);
            return Ok(transaction);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var transactions = await _transactionService.GetTransactionsAsync(accountId);
            return Ok(transactions);
        }
    }
}
