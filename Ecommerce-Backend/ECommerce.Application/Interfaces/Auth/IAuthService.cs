using ECommerce.Application.DTOs.Account;

namespace ECommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<AuthResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
        Task LogoutAsync(string refreshToken);
    }
}