using System;

namespace Clean.Infrastructure.OutBox;

public class OutBoxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime? ProcessedOnUtc { get; set; }
    public DateTime? OccuredOnUtc { get; set; }
    public string? Errors { get; set; }
}
