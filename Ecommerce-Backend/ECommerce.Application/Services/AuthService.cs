using System.Security.Claims;
using ECommerce.Application.DTOs.Account;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepo, IPasswordHasherService passwordHasher, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return new AuthResponseDto { Succeeded = false, Message = "Passwords do not match." };

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                return new AuthResponseDto { Succeeded = false, Message = "Email already registered." };

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return new AuthResponseDto { Succeeded = true, Message = "Registration successful." };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
                return new AuthResponseDto { Succeeded = false, Message = "Invalid credentials." };

            if (!user.IsActive)
                return new AuthResponseDto { Succeeded = false, Message = "Account deactivated." };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("fullName", user.FullName)
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // config
                CreatedAt = DateTime.UtcNow
            });
            await _userRepo.SaveChangesAsync();

            return new AuthResponseDto
            {
                Succeeded = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);
            if (user == null) return new AuthResponseDto { Succeeded = false, Message = "Invalid token." };

            var storedToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
            if (storedToken == null || storedToken.RevokedAt != null || storedToken.ExpiresAt < DateTime.UtcNow)
                return new AuthResponseDto { Succeeded = false, Message = "Token expired or revoked." };

            // Revoke old token
            storedToken.RevokedAt = DateTime.UtcNow;

            // Generate new tokens
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("fullName", user.FullName)
            };
            var newAccessToken = _jwtService.GenerateAccessToken(claims);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });

            await _userRepo.SaveChangesAsync();

            return new AuthResponseDto
            {
                Succeeded = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);
            if (user == null) return;

            var storedToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
            if (storedToken != null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                await _userRepo.SaveChangesAsync();
            }
        }
    }
}