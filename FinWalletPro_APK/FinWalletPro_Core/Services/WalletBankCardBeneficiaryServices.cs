using AutoMapper;
using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    // ─── Wallet Service ───────────────────────────────────────
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<WalletDto>> GetWalletAsync(Guid userId)
        {
            try
            {
                var repo = (IWalletRepository)_uow.Wallets;
                var wallet = await repo.GetByUserIdAsync(userId)
                    ?? throw new WalletNotFoundException(userId);

                return ServiceResult<WalletDto>.Ok(_mapper.Map<WalletDto>(wallet));
            }
            catch (DomainException ex)
            {
                return ServiceResult<WalletDto>.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult<WalletBalanceDto>> GetBalanceAsync(Guid userId)
        {
            try
            {
                var repo = (IWalletRepository)_uow.Wallets;
                var wallet = await repo.GetByUserIdAsync(userId)
                    ?? throw new WalletNotFoundException(userId);

                return ServiceResult<WalletBalanceDto>.Ok(_mapper.Map<WalletBalanceDto>(wallet));
            }
            catch (DomainException ex)
            {
                return ServiceResult<WalletBalanceDto>.Fail(ex.Message);
            }
        }
    }

    // ─── BankCard Service ─────────────────────────────────────
    public class BankCardService : IBankCardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BankCardService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<System.Collections.Generic.List<BankCardDto>>> GetCardsAsync(Guid userId)
        {
            var repo = (IBankCardRepository)_uow.BankCards;
            var cards = await repo.GetActiveCardsByUserIdAsync(userId);
            return ServiceResult<System.Collections.Generic.List<BankCardDto>>.Ok(
                _mapper.Map<System.Collections.Generic.List<BankCardDto>>(cards));
        }

        public async Task<ServiceResult<BankCardDto>> AddCardAsync(Guid userId, AddBankCardDto dto)
        {
            try
            {
                var card = _mapper.Map<BankCard>(dto);
                card.UserId = userId;
                // TODO: Encrypt card number before saving
                card.CardNumber = EncryptCardNumber(dto.CardNumber);

                // If this is first card, make it default
                var repo = (IBankCardRepository)_uow.BankCards;
                var existing = await repo.GetActiveCardsByUserIdAsync(userId);
                if (!System.Linq.Enumerable.Any(existing)) card.IsDefault = true;

                await _uow.BankCards.AddAsync(card);
                await _uow.SaveAsync();

                return ServiceResult<BankCardDto>.Ok(
                    _mapper.Map<BankCardDto>(card),
                    "Card added successfully");
            }
            catch
            {
                return ServiceResult<BankCardDto>.Fail("Failed to add card");
            }
        }

        public async Task<ServiceResult<bool>> DeleteCardAsync(Guid userId, Guid cardId)
        {
            try
            {
                var card = await _uow.BankCards.GetByIdAsync(cardId);
                if (card == null || card.UserId != userId)
                    return ServiceResult<bool>.Fail("Card not found");

                card.IsActive = false;
                card.UpdatedAt = DateTime.UtcNow;
                await _uow.BankCards.UpdateAsync(card);
                await _uow.SaveAsync();

                return ServiceResult<bool>.Ok(true, "Card removed");
            }
            catch
            {
                return ServiceResult<bool>.Fail("Failed to remove card");
            }
        }

        private static string EncryptCardNumber(string cardNumber)
        {
            // Placeholder — integrate AES or a vault service in production
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cardNumber));
        }
    }

    // ─── Beneficiary Service ──────────────────────────────────
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BeneficiaryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<System.Collections.Generic.List<BeneficiaryDto>>> GetBeneficiariesAsync(Guid userId)
        {
            var repo = (IBeneficiaryRepository)_uow.Beneficiaries;
            var benes = await repo.GetActiveByUserIdAsync(userId);
            return ServiceResult<System.Collections.Generic.List<BeneficiaryDto>>.Ok(
                _mapper.Map<System.Collections.Generic.List<BeneficiaryDto>>(benes));
        }

        public async Task<ServiceResult<BeneficiaryDto>> AddBeneficiaryAsync(Guid userId, AddBeneficiaryDto dto)
        {
            try
            {
                // Verify the wallet exists
                var walletRepo = (IWalletRepository)_uow.Wallets;
                var wallet = await walletRepo.GetByWalletNumberAsync(dto.WalletNumber);
                if (wallet == null)
                    return ServiceResult<BeneficiaryDto>.Fail("Wallet number not found");

                var bene = _mapper.Map<Beneficiary>(dto);
                bene.UserId = userId;
                await _uow.Beneficiaries.AddAsync(bene);
                await _uow.SaveAsync();

                return ServiceResult<BeneficiaryDto>.Ok(
                    _mapper.Map<BeneficiaryDto>(bene),
                    "Beneficiary added");
            }
            catch
            {
                return ServiceResult<BeneficiaryDto>.Fail("Failed to add beneficiary");
            }
        }

        public async Task<ServiceResult<bool>> DeleteBeneficiaryAsync(Guid userId, Guid beneficiaryId)
        {
            try
            {
                var bene = await _uow.Beneficiaries.GetByIdAsync(beneficiaryId);
                if (bene == null || bene.UserId != userId)
                    return ServiceResult<bool>.Fail("Beneficiary not found");

                bene.IsActive = false;
                bene.UpdatedAt = DateTime.UtcNow;
                await _uow.Beneficiaries.UpdateAsync(bene);
                await _uow.SaveAsync();

                return ServiceResult<bool>.Ok(true, "Beneficiary removed");
            }
            catch
            {
                return ServiceResult<bool>.Fail("Failed to remove beneficiary");
            }
        }
    }
}
