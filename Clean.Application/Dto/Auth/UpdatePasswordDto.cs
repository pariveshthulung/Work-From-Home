namespace Clean.Application.Dto.Auth;

public record UpdatePasswordDto
{
    public string Email { get; init; } = default!;
    public string CurrentPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
}
