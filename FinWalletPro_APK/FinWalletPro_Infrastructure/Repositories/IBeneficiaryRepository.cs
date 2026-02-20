using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public interface IBeneficiaryRepository
    {
        Task<Beneficiary> CreateAsync(Beneficiary beneficiary);
        Task<Beneficiary> GetByIdAsync(long beneficiaryId);
        Task<IEnumerable<Beneficiary>> GetByAccountIdAsync(long accountId);
        Task<Beneficiary> UpdateAsync(Beneficiary beneficiary);
        Task<bool> DeleteAsync(long beneficiaryId);
        Task<bool> ExistsAsync(long accountId, string accountNumber);
    }
}
