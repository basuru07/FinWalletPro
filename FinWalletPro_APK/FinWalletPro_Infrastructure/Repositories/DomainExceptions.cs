namespace FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Insufficient Balance Exception
    public class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException()
            : base("Insufficient wallet balance for this transaction") { }

        public InsufficientBalanceException(decimal available, decimal required)
            :base($"Insufficient balance. Available: {available}, Required: {required}") { }
    }

    // Wallet Not Found
    public class WalletNotFoundException : DomainException
    {
        public WalletNotFoundException(Guid walletId)
            :base($"Wallet with ID {walletId} was not found") { }

        public WalletNotFoundException(string walletNumber)
            :base($"Wallet with number {walletNumber} was not found") { }
    }

    // User not found Exception
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId)
            : base($"User with ID {userId} was not found") { }

        public UserNotFoundException(string email)
            : base($"User with email {email} was not found") { }
    }

    // Transaction not found Exception
    public class TransactionNotFoundException : DomainException
    {
        public TransactionNotFoundException(Guid transactionId)
            : base($"Transaction with ID {transactionId} was not found") { }

        public TransactionNotFoundException(string reference)
            : base($"Transaction with reference {reference} was not found") { }
    }

    // Invalid Transaction Exception
    public class InvalidTransactionException : DomainException
    {
        public InvalidTransactionException(string message) : base(message) { }
    }

    // Wallet Inactive Exception
    public class WalletInactiveException : DomainException
    {
        public WalletInactiveException()
            : base("Wallet is inactive and cannot perform transactions") { }
    }

    public class BankCardNotFoundException : DomainException
    {
        public BankCardNotFoundException(Guid cardId)
            : base($"Bank card with ID '{cardId}' was not found") { }
    }

    public class InvalidBankCardException : DomainException
    {
        public InvalidBankCardException(string message) : base(message) { }
    }

    // Duplicate Email Exception
    public class DuplicateEmailException : DomainException
    {
        public DuplicateEmailException(string email)
            : base($"User with email {email} already exists") { }
    }

}
