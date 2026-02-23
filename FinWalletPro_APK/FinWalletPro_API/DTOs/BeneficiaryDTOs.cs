using System.ComponentModel.DataAnnotations;

namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    // Add_Beneficiary_RequestDto
    public class AddBeneficiaryRequestDto
    {
        [Required][MaxLength(200)] public string? BeneficiaryName { get; set; }
        [Required][MaxLength(50)] public string? BeneficiaryAccountNumber { get; set; }
        [MaxLength(100)] public string? NickName { get; set; }
        [EmailAddress][MaxLength(200)] public string? BeneficiaryEmail { get; set; }
        [Phone][MaxLength(20)] public string? BeneficiaryPhone { get; set; }
        [MaxLength(100)] public string? BankName { get; set; }
    }

    // Update_Beneficiary_RequestDto
    public class UpdateBeneficiaryRequestDto
    {
        [MaxLength(100)] public string? NickName { get; set; }
        [MaxLength(200)] public string? BeneficiaryName { get; set; }
        [Phone][MaxLength(20)] public string? BeneficiaryPhone { get; set; }
        [MaxLength(100)] public string? BankName { get; set; }
    }

    // Beneficiary_ResponseDto
    public class BeneficiaryResponseDto
    {
        public long BeneficiaryId { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }
        public string? NickName { get; set; }
        public string? BeneficiaryEmail { get; set; }
        public string? BeneficiaryPhone { get; set; }
        public string? BankName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
