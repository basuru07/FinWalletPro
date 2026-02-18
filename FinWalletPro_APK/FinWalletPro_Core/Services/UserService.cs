using AutoMapper;
using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Interface;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using FinWalletPro_APK.FinWalletPro_Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity.Data;

namespace FinWalletPro_APK.FinWalletPro_Core.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(IUnitOfWork uow, IMapper mapper, IJwtService jwtService)
        {
            _uow = uow;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        // ── Register ─────────────────────────────────────────
        public async Task<ServiceResult<UserDto>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userRepo = (IUserRepository)_uow.Users;

                if (await userRepo.EmailExistsAsync(request.Email))
                    throw new DuplicateEmailException(request.Email);

                await _uow.BeginTransactionAsync();

                var user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = BC.HashPassword(request.Password)
                };
                await _uow.Users.AddAsync(user);

                // Auto-create wallet for new user
                var wallet = new Wallet
                {
                    UserId = user.Id,
                    WalletNumber = GenerateWalletNumber()
                };
                await _uow.Wallets.AddAsync(wallet);
                user.Wallet = wallet;

                await _uow.CommitTransactionAsync();

                return ServiceResult<UserDto>.Ok(
                    _mapper.Map<UserDto>(user),
                    "Registration successful");
            }
            catch (DomainException ex)
            {
                await _uow.RollbackTransactionAsync();
                return ServiceResult<UserDto>.Fail(ex.Message);
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                return ServiceResult<UserDto>.Fail("Registration failed. Please try again.");
            }
        }

        // ── Login ─────────────────────────────────────────────
        public async Task<ServiceResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var userRepo = (IUserRepository)_uow.Users;
                var user = await userRepo.GetByEmailAsync(request.Email);

                if (user == null || !BC.Verify(request.Password, user.PasswordHash))
                    return ServiceResult<LoginResponse>.Fail("Invalid email or password");

                if (!user.IsActive)
                    return ServiceResult<LoginResponse>.Fail("Account is deactivated");

                user.LastLoginAt = DateTime.UtcNow;
                await _uow.Users.UpdateAsync(user);
                await _uow.SaveAsync();

                var response = new LoginResponse
                {
                    Token = _jwtService.GenerateToken(user),
                    RefreshToken = _jwtService.GenerateRefreshToken(),
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = _mapper.Map<UserDto>(user)
                };

                return ServiceResult<LoginResponse>.Ok(response, "Login successful");
            }
            catch
            {
                return ServiceResult<LoginResponse>.Fail("Login failed. Please try again.");
            }
        }

        // ── Get Profile ───────────────────────────────────────
        public async Task<ServiceResult<UserDto>> GetProfileAsync(Guid userId)
        {
            try
            {
                var userRepo = (IUserRepository)_uow.Users;
                var user = await userRepo.GetWithWalletAsync(userId)
                    ?? throw new UserNotFoundException(userId);

                return ServiceResult<UserDto>.Ok(_mapper.Map<UserDto>(user));
            }
            catch (DomainException ex)
            {
                return ServiceResult<UserDto>.Fail(ex.Message);
            }
        }

        // ── Update Profile ────────────────────────────────────
        public async Task<ServiceResult<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserProfileDto dto)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(userId)
                    ?? throw new UserNotFoundException(userId);

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                await _uow.Users.UpdateAsync(user);
                await _uow.SaveAsync();

                return ServiceResult<UserDto>.Ok(_mapper.Map<UserDto>(user), "Profile updated");
            }
            catch (DomainException ex)
            {
                return ServiceResult<UserDto>.Fail(ex.Message);
            }
        }

        // ── Helper ────────────────────────────────────────────
        private static string GenerateWalletNumber()
            => $"FW{DateTime.UtcNow:yyyyMMddHHmm}{new Random().Next(100, 999)}";
    }
}
