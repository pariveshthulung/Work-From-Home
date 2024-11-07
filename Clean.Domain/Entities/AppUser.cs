using Microsoft.AspNetCore.Identity;

namespace Clean.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public AppUser() { }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool IsPasswordExpire { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void SetIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public void SetPasswordExpire(bool expire)
    {
        IsPasswordExpire = expire;
    }
}
