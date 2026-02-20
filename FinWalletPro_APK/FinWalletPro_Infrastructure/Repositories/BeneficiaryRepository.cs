using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly WalletDbContext _context;

        public BeneficiaryRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Beneficiary> CreateAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<Beneficiary> GetByIdAsync(long beneficiaryId)
            => await _context.Beneficiaries.FindAsync(beneficiaryId);

        public async Task<IEnumerable<Beneficiary>> GetByAccountIdAsync(long accountId)
            => await _context.Beneficiaries
                .Where(b => b.AccountId == accountId && b.IsActive)
                .OrderBy(b => b.BeneficiaryName)
                .ToListAsync();

        public async Task<Beneficiary> UpdateAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Update(beneficiary);
            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<bool> DeleteAsync(long beneficiaryId)
        {
            var b = await _context.Beneficiaries.FindAsync(beneficiaryId);
            if (b == null) return false;
            _context.Beneficiaries.Remove(b);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long accountId, string accountNumber)
            => await _context.Beneficiaries
                .AnyAsync(b => b.AccountId == accountId
                    && b.BeneficiaryAccountNumber == accountNumber
                    && b.IsActive);
    }
}
