using System;
using System.Security.Claims;
using Clean.Application.Persistence.Contract;
using Microsoft.AspNetCore.Http;

namespace Clean.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserEmail =>
        _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
        _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
