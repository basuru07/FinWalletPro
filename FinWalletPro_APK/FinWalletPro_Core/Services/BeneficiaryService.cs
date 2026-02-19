using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly BeneficiaryRepository _repo;

        public BeneficiaryService(BeneficiaryRepository repo)
        {
            _repo = repo;
        }

        public async Task AddBeneficiaryAsync(int accountId, string name, string bankAccountNumber)
        {
            var beneficiary = new Beneficiary
            {
                AccountId = accountId,
                Name = name,
                BankAccountNumber = bankAccountNumber
            };

            await _repo.AddAsync(beneficiary);
        }

        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int accountId)
        {
            return await _repo.GetByAccountIdAsync(accountId);
        }
    }
}
