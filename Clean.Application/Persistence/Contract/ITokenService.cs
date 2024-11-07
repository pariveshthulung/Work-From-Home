using System.Security.Claims;
using Clean.Domain.Entities;

namespace Clean.Application.Persistence.Contract;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(AppUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string Token);
}
