namespace Clean.Application.Dto.Auth;

public record UpdatePasswordDto
{
    public string Email { get; init; }
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmPassword { get; init; }
}
