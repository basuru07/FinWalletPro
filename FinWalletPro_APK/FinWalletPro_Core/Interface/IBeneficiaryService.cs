using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public class IBeneficiaryService
    {
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary);
        Task<List<Beneficiary>> GetBeneficiariesAsync(int accountId);
    }
}
