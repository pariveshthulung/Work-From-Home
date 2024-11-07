using Clean.Application.Dto.Address;

namespace Clean.Application.Dto.Employee;

public record RegisterEmployeeDto : IEmployeeDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public int UserRoleId { get; init; }
    public string PhoneNumber { get; init; }
    public AddressDto Address { get; init; }

    public Domain.Entities.Employee ToEmployee()
    {
        throw new NotImplementedException();
    }
}
