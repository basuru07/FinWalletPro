using Amazon.Runtime.Internal;
using FinWalletPro_APK.FinWalletPro_API.DTOs;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Register Command
    public class RegisterUserCommand
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    // Add Beneficiary Command
    public class AddBeneficiaryCommand
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
    }
}