using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public AccountService(
            IAccountRepository accountRepository, 
            IConfiguration configuration,
            INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        // Register 
        public async Task<Account> RegisterAsync(Account account, string password)
        {
            var existing = await _accountRepository.GetByEmailAsync(account.Email);
            if (existing != null)
                throw new InvalidOperationException("An account with this email already exists.");

            account.PasswordHash = HashPassword(password);
            account.AccountNumber = GenerateAccountNumber();
            account.UserId = Guid.NewGuid().ToString();
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;
            account.Balance = 0;
            account.AccountStatus = "Active";

            var created = await _accountRepository.CreateAsync(account);

            await _notificationService.CreateNotificationAsync(new Notification
            {
                AccountId = created.AccountId,
                Title = "Welcome to FinWalletPro!",
                Message = $"Your account has been created successfully. Account Number: {created.AccountNumber}",
                NotificationType = "System",
                CreatedAt = DateTime.UtcNow
            });

            return created;
        }

        // Login
        public async Task<(Account account, string accessToken, string refreshToken)> LoginAsync(string email, string password)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account == null || !VerifyPassword(password, account.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (account.AccountStatus != "Active")
                throw new UnauthorizedAccessException($"Account is {account.AccountStatus}. Please contact support.");

            var accessToken = GenerateJwtToken(account);
            var refreshToken = GenerateRefreshToken();

            account.RefreshToken = refreshToken;
            account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            await _notificationService.SendSecurityNotificationAsync(
                account.AccountId,
                "New Login Detected",
                $"A login was detected on your account at {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC.");

            return (account, accessToken, refreshToken);
        }

        // Refresh Token
        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var account = await _accountRepository.GetByRefreshTokenAsync(refreshToken);
            if (account == null || account.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            var newAccessToken = GenerateJwtToken(account);
            var newRefreshToken = GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            return (newAccessToken, newRefreshToken);
        }

        // LogOut
        public async Task<bool> LogoutAsync(long accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return false;

            account.RefreshToken = null;
            account.RefreshTokenExpiry = null;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        // Get Account by ID
        public async Task<Account> GetAccountByIdAsync(long accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) throw new KeyNotFoundException("Account not found.");
            return account;
        }

        // Get Account By Email
        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account == null) throw new KeyNotFoundException("Account not found.");
            return account;
        }

        // Update Account
        public async Task<Account> UpdateAccountAsync(long accountId, Account updatedAccount)
        {
            var account = await GetAccountByIdAsync(accountId);
            account.FullName = updatedAccount.FullName ?? account.FullName;
            account.PhoneNumber = updatedAccount.PhoneNumber ?? account.PhoneNumber;
            account.UpdatedAt = DateTime.UtcNow;
            return await _accountRepository.UpdateAsync(account);
        }

        // Change Password
        public async Task<bool> ChangePasswordAsync(long accountId, string currentPassword, string newPassword)
        {
            var account = await GetAccountByIdAsync(accountId);
            if (!VerifyPassword(currentPassword, account.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            account.PasswordHash = HashPassword(newPassword);
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            await _notificationService.SendSecurityNotificationAsync(
                accountId,
                "Password Changed",
                "Your account password was changed successfully. Contact support if this wasn't you.");

            return true;
        }

        // Get Balanced
        public async Task<decimal> GetBalanceAsync(long accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            return account.Balance;
        }

        // Get All Account
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
            => await _accountRepository.GetAllAsync();

        // Suspend Account
        public async Task<bool> SuspendAccountAsync(long accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            account.AccountStatus = "Suspended";
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        // Activate Account
        public async Task<bool> ActivateAccountAsync(long accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            account.AccountStatus = "Active";
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        // Add BankCard
        public async Task<BankCard> AddBankCardAsync(BankCard card)
        {
            card.LinkedAt = DateTime.UtcNow;
            card.UpdatedAt = DateTime.UtcNow;

            var existingCards = await _accountRepository.GetBankCardsAsync(card.AccountId);
            bool isFirst = !existingCards.Any();
            if (isFirst) card.IsDefault = true;

            return await _accountRepository.AddBankCardAsync(card);
        }

        // Get BankCard
        public async Task<IEnumerable<BankCard>> GetBankCardsAsync(long accountId)
            => await _accountRepository.GetBankCardsAsync(accountId);

        public async Task<bool> RemoveBankCardAsync(long accountId, long cardId)
            => await _accountRepository.RemoveBankCardAsync(accountId, cardId);

        public async Task<bool> SetDefaultCardAsync(long accountId, long cardId)
            => await _accountRepository.SetDefaultCardAsync(accountId, cardId);

        // Private helpers
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var saltedPassword = $"FinWallet_{password}_Salt2024";
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
            => HashPassword(password) == hash;

        private string GenerateJwtToken(Account account)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("accountNumber", account.AccountNumber),
                new Claim("fullName", account.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static string GenerateAccountNumber()
        {
            var rng = new Random();
            return $"FWP{DateTime.UtcNow:yyyyMMdd}{rng.Next(1000, 9999)}";
        }
    }
}

