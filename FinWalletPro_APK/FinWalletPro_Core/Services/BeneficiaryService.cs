using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository)
        {
            _beneficiaryRepository = beneficiaryRepository;
        }

        public async Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary)
        {
            var exists = await BeneficiaryExistsAsync(beneficiary.AccountId, beneficiary.BeneficiaryAccountNumber);
            if (exists)
                throw new InvalidOperationException("This beneficiary is already added.");

            beneficiary.CreatedAt = DateTime.UtcNow;
            beneficiary.UpdatedAt = DateTime.UtcNow;
            beneficiary.IsActive = true;
            return await _beneficiaryRepository.CreateAsync(beneficiary);
        }

        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(long accountId)
            => await _beneficiaryRepository.GetByAccountIdAsync(accountId);

        public async Task<Beneficiary> GetBeneficiaryByIdAsync(long beneficiaryId)
        {
            var b = await _beneficiaryRepository.GetByIdAsync(beneficiaryId);
            if (b == null) throw new KeyNotFoundException("Beneficiary not found.");
            return b;
        }

        public async Task<Beneficiary> UpdateBeneficiaryAsync(long beneficiaryId, Beneficiary updated)
        {
            var beneficiary = await GetBeneficiaryByIdAsync(beneficiaryId);
            beneficiary.NickName = updated.NickName ?? beneficiary.NickName;
            beneficiary.BeneficiaryName = updated.BeneficiaryName ?? beneficiary.BeneficiaryName;
            beneficiary.BeneficiaryPhone = updated.BeneficiaryPhone ?? beneficiary.BeneficiaryPhone;
            beneficiary.BankName = updated.BankName ?? beneficiary.BankName;
            beneficiary.UpdatedAt = DateTime.UtcNow;
            return await _beneficiaryRepository.UpdateAsync(beneficiary);
        }

        public async Task<bool> RemoveBeneficiaryAsync(long accountId, long beneficiaryId)
        {
            var beneficiary = await GetBeneficiaryByIdAsync(beneficiaryId);
            if (beneficiary.AccountId != accountId)
                throw new UnauthorizedAccessException("You don't own this beneficiary.");
            return await _beneficiaryRepository.DeleteAsync(beneficiaryId);
        }

        public async Task<bool> BeneficiaryExistsAsync(long accountId, string accountNumber)
            => await _beneficiaryRepository.ExistsAsync(accountId, accountNumber);
    }
}
