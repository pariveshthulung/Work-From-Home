namespace Clean.Domain.Entities;

public class GeneralEmployee : Employee
{
    private GeneralEmployee() { }

    private GeneralEmployee(
        string name,
        string email,
        string phoneNumber,
        int applicationRoleId,
        Address address
    )
        : base(name, email, phoneNumber, applicationRoleId, address) { }

    public static GeneralEmployee Create(
        string name,
        string email,
        string phoneNumber,
        int applicationRoleId,
        Address address
    )
    {
        return new GeneralEmployee(name, email, phoneNumber, applicationRoleId, address);
    }
}
