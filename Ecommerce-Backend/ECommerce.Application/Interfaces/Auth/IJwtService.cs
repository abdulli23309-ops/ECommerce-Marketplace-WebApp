using System.Security.Claims;

namespace ECommerce.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateExpiredToken(string token);
    }
}