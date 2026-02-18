using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    // Wallet Service interface
    public interface IWalletService
    {
        Task<ServiceResult<WalletDto>> GetWalletAsync(Guid userId);
        Task<ServiceResult<WalletBalanceDto>> GetBalanceAsync(Guid userId);
    }

    // Transaction Service interface
    public interface ITransactionService
    {
        Task<ServiceResult<TransactionDto>> TransferAsync(TransferCommand command);
        Task<ServiceResult<TransactionDto>> DepositAsync(DepositCommand command);
        Task<ServiceResult<TransactionDto>> WithdrawAsync(WithdrawCommand command);
        Task<ServiceResult<TransactionHistoryDto>> GetHistoryAsync(Guid userId, TransactionFilter filter);
        Task<ServiceResult<TransactionDto>> GetByReferenceAsync(string reference);
    }

    // Bank Card interface
    public interface IBankCardService
    {
        Task<ServiceResult<BankCardDto>> AddCardAsync(Guid userId, AddBankCardDto dto);
        Task<ServiceResult<System.Collections.Generic.List<BankCardDto>>> GetCardsAsync(Guid userId);
        Task<ServiceResult<bool>> DeleteCardAsync(Guid userId, Guid cardId);
    }

    // Beneficiary interface
    public interface IBeneficiaryService
    {
        Task<ServiceResult<BeneficiaryDto>> AddBeneficiaryAsync(Guid userId, AddBeneficiaryDto dto);
        Task<ServiceResult<System.Collections.Generic.List<BeneficiaryDto>>> GetBeneficiariesAsync(Guid userId);
        Task<ServiceResult<bool>> DeleteBeneficiaryAsync(Guid userId, Guid beneficiaryId);
    }

    // JWT interface
    public interface IJwtService
    {
        string GenerateToken(Models.User user);
        string GenerateRefreshToken();
        Guid? ValidateToken(string token);
    }
}
