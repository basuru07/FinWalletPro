using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Models;

namespace FinWalletPro_APK.FinWalletPro_Infrastructure
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // ── User / Auth ───────────────────────────────────
            CreateMap<User, UserDto>()
                .ForMember(d => d.Wallet, o => o.MapFrom(s => s.Wallet));

            CreateMap<RegisterUserCommand, User>()
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());

            // ── Wallet ────────────────────────────────────────
            CreateMap<Wallet, WalletDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<Wallet, WalletBalanceDto>()
                .ForMember(d => d.PendingAmount,
                           o => o.MapFrom(s => s.Balance - s.AvailableBalance));

            // ── Transaction ───────────────────────────────────
            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            // ── BankCard ──────────────────────────────────────
            CreateMap<BankCard, BankCardResponse>();

            CreateMap<AddBankCardRequest, BankCard>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CardNumber, o => o.Ignore()) // Encrypted separately
                .ForMember(d => d.Last4Digits,
                           o => o.MapFrom(s => s.CardNumber.Substring(s.CardNumber.Length - 4)));

            // ── Beneficiary ───────────────────────────────────
            CreateMap<Beneficiary, BeneficiaryDto>();

            CreateMap<AddBeneficiaryCommand, Beneficiary>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());
        }
    }
}
