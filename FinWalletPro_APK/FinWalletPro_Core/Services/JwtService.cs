using FinWalletPro_APK.FinWalletPro_Core.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        // ── Generate Access Token ─────────────────────────────
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Secret"]));
            var handler = new JwtSecurityTokenHandler();

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email,          user.Email),
                    new Claim(ClaimTypes.Name,           $"{user.FirstName} {user.LastName}"),
                    new Claim("WalletId",                user.Wallet?.Id.ToString() ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }

        // ── Generate Refresh Token ────────────────────────────
        public string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        // ── Validate Token → returns UserId ──────────────────
        public Guid? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            try
            {
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
                var handler = new JwtSecurityTokenHandler();

                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validated);

                var jwt = (JwtSecurityToken)validated;
                var userId = jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return Guid.Parse(userId);
            }
            catch
            {
                return null;
            }
        }
    }
}
