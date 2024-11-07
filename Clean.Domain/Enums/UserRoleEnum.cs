namespace Clean.Domain.Enums;

public class UserRoleEnum : Enumeration
{
    private UserRoleEnum(int id, string name)
        : base(id, name) { }

    public static readonly UserRoleEnum SuperAdmin = new UserRoleEnum(1, "SuperAdmin");
    public static readonly UserRoleEnum Admin = new UserRoleEnum(2, "Admin");
    public static readonly UserRoleEnum Ceo = new UserRoleEnum(3, "Ceo");
    public static readonly UserRoleEnum Manager = new UserRoleEnum(4, "Manager");
    public static readonly UserRoleEnum Intern = new UserRoleEnum(5, "Intern");
    public static readonly UserRoleEnum Developer = new UserRoleEnum(6, "Developer");

    private static readonly List<UserRoleEnum> _roles = new List<UserRoleEnum>
    {
        SuperAdmin,
        Admin,
        Ceo,
        Manager,
        Intern,
        Developer
    };
    public static IEnumerable<UserRoleEnum> Roles => _roles;

    public static UserRoleEnum FromId(int id)
    {
        return _roles.FirstOrDefault(x => x.Id == id)
            ?? throw new ArgumentException($"No Role with Id {id} is found.");
    }

    public static UserRoleEnum FromName(string name)
    {
        return _roles.FirstOrDefault(x =>
                x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
            ) ?? throw new ArgumentException($"No Role with {name} is found.");
    }
}
