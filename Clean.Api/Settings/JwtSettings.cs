using System;

namespace Clean.Api.Settings;

public class JwtSettings
{
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
    public string? SigningKey { get; init; }
    public int ExpiryMinutes { get; init; }
}
