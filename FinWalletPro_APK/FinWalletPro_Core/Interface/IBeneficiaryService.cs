using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public interface IBeneficiaryService
    {
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(long accountId);
        Task<Beneficiary> GetBeneficiaryByIdAsync(long beneficiaryId);
        Task<Beneficiary> UpdateBeneficiaryAsync(long beneficiaryId, Beneficiary updated);
        Task<bool> RemoveBeneficiaryAsync(long accountId, long beneficiaryId);
        Task<bool> BeneficiaryExistsAsync(long accountId, string accountNumber);
    }
}
