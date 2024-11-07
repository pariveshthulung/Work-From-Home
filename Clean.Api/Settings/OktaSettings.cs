using System;

namespace Clean.Api.Settings;

public class OktaSettings
{
    public string? OktaDomain { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}
