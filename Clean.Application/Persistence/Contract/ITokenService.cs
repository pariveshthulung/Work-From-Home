using System.Security.Claims;
using Clean.Domain.Entities;

namespace Clean.Application.Persistence.Contract;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(AppUser user);
    string GenerateRefreshToken();
    bool IsTokenExpired(string token);

    // Task<string> GenerateNewAccessTokenAsync(string accessToken, string refreshToken);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string Token);
}
