using Clean.Application.Dto.Auth;
using Clean.Application.Dto.Token;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clean.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class TokenController(ITokenService tokenService, UserManager<AppUser> userManager)
    : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(Tokens tokens)
    {
        try
        {
            if (tokens.AccessToken is null || tokens.RefreshToken is null)
                return BadRequest("Invalid client request");
            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity?.Name;
            var user = userManager.Users.FirstOrDefault(x => x.UserName == userName);
            if (
                user is null
                || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.Now
            )
                return BadRequest("Invalid client request");
            var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Could not update token.");
            }
            return Ok(
                new AuthResponse() { AccessToken = newAccessToken, RefreshToken = newRefreshToken }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("revoke"), Authorize]
    public async Task<IActionResult> Revoke()
    {
        var userName = User.Identity?.Name;
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return Unauthorized("user is not authenticated.");
        user.RefreshToken = string.Empty;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return NoContent();
    }
}
