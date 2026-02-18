using FinWalletPro_APK.FinWalletPro_API.DTOs;
using FinWalletPro_APK.FinWalletPro_Core.Models;
using System;
using System.Threading.Tasks;
using UserDto = FinWalletPro_APK.FinWalletPro_API.DTOs.UserDto;

namespace FinWalletPro_APK.FinWalletPro_Core.Interface
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> RegisterAsync(RegisterRequest request);
        Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequest request);
        Task<ServiceResult<UserDto>> GetProfileAsync(Guid userId);
        Task<ServiceResult<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserProfileDto dto);
    }

    // ServiceResult Class
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
