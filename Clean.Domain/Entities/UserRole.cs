using Microsoft.AspNetCore.Identity;

namespace Clean.Domain.Entities;

public class UserRole : IdentityRole<int>
{
    private UserRole() { }

    private UserRole(int id, string role)
    {
        Id = id;
        Name = role;
    }

    public static UserRole Create(int id, string role)
    {
        return new UserRole(id, role);
    }
}
