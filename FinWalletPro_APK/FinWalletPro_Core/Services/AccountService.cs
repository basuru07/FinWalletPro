using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using Microsoft.IdentityModel.Tokens;
using Octopus.Client.Repositories;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IConfiguration _config;

        public AccountService(IAccountRepository accountRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            _config = config;
        }

        public async Task<Account> RegisterAsync(Account account, string password)
        {
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return await _accountRepo.AddAsync(account);
        }

        public async Task<Account?> GetByIdAsync(int accountId)
            => await _accountRepo.GetByIdAsync(accountId);

        public async Task<Account?> GetByEmailAsync(string email)
            => await _accountRepo.GetByEmailAsync(email);

        public async Task<decimal> GetBalanceAsync(int accountId)
        {
            var account = await _accountRepo.GetByIdAsync(accountId);
            return account?.Balance ?? 0;
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Name, account.FullName)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

