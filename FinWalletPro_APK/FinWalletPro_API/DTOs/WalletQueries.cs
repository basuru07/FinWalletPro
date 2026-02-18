using Amazon.Runtime.Internal;
using FinWalletPro_APK.FinWalletPro_API.DTOs;
using Microsoft.AspNetCore.Authentication;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    public class GetWalletBalanceQuery
    { 
        public Guid UserId { get; set; }
    }

    public class GetTransactionHistoryQuery
    {
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public int PageNumber {  get; set; }
        public int PageSize { get; set; }
    }

    // Get Transaction by Reference Query
    public class GetTransactionByReferenceQuery
    {
        public string? TransactionReference { get; set; }
    }
}





