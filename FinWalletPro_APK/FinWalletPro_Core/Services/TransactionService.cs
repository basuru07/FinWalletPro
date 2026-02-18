using AutoMapper;
using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    // ── Transfer ──────────────────────────────────────────
    public async Task<ServiceResult<TransactionDto>> TransferAsync(Models.TransferCommand command)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var walletRepo = (IWalletRepository)_uow.Wallets;

            var source = await walletRepo.GetByUserIdAsync(command.UserId)
                ?? throw new WalletNotFoundException(command.UserId);

            var destination = await walletRepo.GetByWalletNumberAsync(command.DestinationWalletNumber)
                ?? throw new WalletNotFoundException(command.DestinationWalletNumber);

            if (source.Id == destination.Id)
                throw new InvalidTransactionException("Cannot transfer to the same wallet");

            var fee = Math.Round(Math.Min(command.Amount * 0.01m, 5m), 2); // 1% capped at $5
            var total = command.Amount + fee;

            if (!source.CanDebit(total))
                throw new InsufficientBalanceException(source.AvailableBalance, total);

            var txn = new Transaction
            {
                SourceWalletId = source.Id,
                DestinationWalletId = destination.Id,
                Amount = command.Amount,
                Fee = fee,
                TotalAmount = total,
                Type = TransactionType.Transfer,
                Description = command.Description
            };

            source.Debit(total);
            destination.Credit(command.Amount);

            await _uow.Wallets.UpdateAsync(source);
            await _uow.Wallets.UpdateAsync(destination);

            txn.MarkAsCompleted();
            await _uow.Transactions.AddAsync(txn);

            await _uow.CommitTransactionAsync();

            return ServiceResult<TransactionDto>.Ok(
                _mapper.Map<TransactionDto>(txn),
                "Transfer completed successfully");
        }
        catch (DomainException ex)
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail(ex.Message);
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail("Transfer failed. Please try again.");
        }
    }

    // ── Deposit ───────────────────────────────────────────
    public async Task<ServiceResult<TransactionDto>> DepositAsync(Models.DepositCommand command)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var walletRepo = (IWalletRepository)_uow.Wallets;
            var wallet = await walletRepo.GetByUserIdAsync(command.UserId)
                ?? throw new WalletNotFoundException(command.UserId);

            var card = await _uow.BankCards.GetByIdAsync(command.BankCardId)
                ?? throw new BankCardNotFoundException(command.BankCardId);

            if (card.UserId != command.UserId)
                throw new InvalidBankCardException("Bank card does not belong to this user");

            // TODO: Integrate real payment gateway here

            var txn = new Transaction
            {
                DestinationWalletId = wallet.Id,
                Amount = command.Amount,
                Fee = 0,
                TotalAmount = command.Amount,
                Type = TransactionType.Deposit,
                Description = command.Description,
                BankCardLast4 = card.Last4Digits
            };

            wallet.Credit(command.Amount);
            await _uow.Wallets.UpdateAsync(wallet);
            txn.MarkAsCompleted();
            await _uow.Transactions.AddAsync(txn);

            await _uow.CommitTransactionAsync();

            return ServiceResult<TransactionDto>.Ok(
                _mapper.Map<TransactionDto>(txn),
                "Deposit successful");
        }
        catch (DomainException ex)
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail(ex.Message);
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail("Deposit failed. Please try again.");
        }
    }

    // ── Withdraw ──────────────────────────────────────────
    public async Task<ServiceResult<TransactionDto>> WithdrawAsync(Models.WithdrawCommand command)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var walletRepo = (IWalletRepository)_uow.Wallets;
            var wallet = await walletRepo.GetByUserIdAsync(command.UserId)
                ?? throw new WalletNotFoundException(command.UserId);

            var fee = Math.Round(Math.Min(command.Amount * 0.02m, 10m), 2); // 2% capped at $10
            var total = command.Amount + fee;

            if (!wallet.CanDebit(total))
                throw new InsufficientBalanceException(wallet.AvailableBalance, total);

            var card = await _uow.BankCards.GetByIdAsync(command.BankCardId)
                ?? throw new BankCardNotFoundException(command.BankCardId);

            // TODO: Integrate real payout gateway here

            var txn = new Transaction
            {
                SourceWalletId = wallet.Id,
                Amount = command.Amount,
                Fee = fee,
                TotalAmount = total,
                Type = TransactionType.Withdrawal,
                Description = command.Description,
                BankCardLast4 = card.Last4Digits
            };

            wallet.Debit(total);
            await _uow.Wallets.UpdateAsync(wallet);
            txn.MarkAsCompleted();
            await _uow.Transactions.AddAsync(txn);

            await _uow.CommitTransactionAsync();

            return ServiceResult<TransactionDto>.Ok(
                _mapper.Map<TransactionDto>(txn),
                "Withdrawal successful");
        }
        catch (DomainException ex)
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail(ex.Message);
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return ServiceResult<TransactionDto>.Fail("Withdrawal failed. Please try again.");
        }
    }

    // ── History ───────────────────────────────────────────
    public async Task<ServiceResult<TransactionHistoryDto>> GetHistoryAsync(
        Guid userId, TransactionFilter filter)
    {
        try
        {
            var walletRepo = (IWalletRepository)_uow.Wallets;
            var wallet = await walletRepo.GetByUserIdAsync(userId)
                ?? throw new WalletNotFoundException(userId);

            var txnRepo = (ITransactionRepository)_uow.Transactions;
            IEnumerable<Transaction> txns;

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
                txns = await txnRepo.GetByDateRangeAsync(wallet.Id, filter.StartDate.Value, filter.EndDate.Value);
            else
                txns = await txnRepo.GetByWalletIdPagedAsync(wallet.Id, filter.PageNumber, filter.PageSize);

            // Apply in-memory filters
            if (!string.IsNullOrEmpty(filter.Type))
                txns = txns.Where(t => t.Type.ToString().Equals(filter.Type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.Status))
                txns = txns.Where(t => t.Status.ToString().Equals(filter.Status, StringComparison.OrdinalIgnoreCase));

            var list = txns.ToList();

            return ServiceResult<TransactionHistoryDto>.Ok(new TransactionHistoryDto
            {
                TotalCount = list.Count,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Transactions = _mapper.Map<List<TransactionDto>>(list)
            });
        }
        catch (DomainException ex)
        {
            return ServiceResult<TransactionHistoryDto>.Fail(ex.Message);
        }
    }

    // ── Get By Reference ──────────────────────────────────
    public async Task<ServiceResult<TransactionDto>> GetByReferenceAsync(string reference)
    {
        try
        {
            var txnRepo = (ITransactionRepository)_uow.Transactions;
            var txn = await txnRepo.GetByReferenceAsync(reference)
                ?? throw new TransactionNotFoundException(reference);

            return ServiceResult<TransactionDto>.Ok(_mapper.Map<TransactionDto>(txn));
        }
        catch (DomainException ex)
        {
            return ServiceResult<TransactionDto>.Fail(ex.Message);
        }
    }
}
