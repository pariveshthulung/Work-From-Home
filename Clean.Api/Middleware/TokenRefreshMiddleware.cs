using System.IdentityModel.Tokens.Jwt;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Clean.Api.Middleware;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        try
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var jwtToken =
                        new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                    if (jwtToken != null)
                    {
                        var isExpire = jwtToken.ValidTo < DateTime.UtcNow;
                        if (isExpire)
                        {
                            var email = jwtToken
                                .Claims.FirstOrDefault(x => x.Type == "email")
                                ?.Value;

                            using (var scope = serviceProvider.CreateScope())
                            {
                                var userManager = scope.ServiceProvider.GetRequiredService<
                                    UserManager<AppUser>
                                >();
                                var tokenService =
                                    scope.ServiceProvider.GetRequiredService<ITokenService>();

                                // Validate and retrieve refresh token from UserManager or wherever it is stored
                                var user = await userManager.FindByEmailAsync(email!);
                                var refreshToken = user?.RefreshToken;
                                var refreshTokenExpiry = user?.RefreshTokenExpiryTime;

                                if (refreshToken != null && refreshTokenExpiry > DateTime.UtcNow)
                                {
                                    // Generate a new access token
                                    var newAccessToken =
                                        await tokenService.GenerateAccessTokenAsync(user!);
                                    context.Request.Headers["Authorization"] =
                                        "Bearer " + newAccessToken;
                                }
                                else
                                {
                                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                    await context.Response.WriteAsync(
                                        "Invalid or expired refresh token."
                                    );
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            await _next(context);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
