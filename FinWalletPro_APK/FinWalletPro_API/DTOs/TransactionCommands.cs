using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Transfer Command 
    public class TransferCommand
    {
        public string? DestinationWalletNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Pin { get; set; }
    }

    // Deposit Money Command
    public class DepositCommand 
    {
        public Guid UserId { get; set; }
        public Guid BankCardId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    // Withdraw Money Command
    public class WithdrawCommand 
    {
        public Guid UserId { get; set;}
        public Guid BankCardId { get; set;}
        public decimal Amount { get; set;}
        public string? Description { get; set; }
        public string? Pin { get; set;}
    }
}