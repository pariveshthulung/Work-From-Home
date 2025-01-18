using Clean.Application.Dto.Address;

namespace Clean.Application.Dto.Employee;

public record RegisterEmployeeDto : IEmployeeDto
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public int UserRoleId { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;

    public Domain.Entities.Employee ToEmployee()
    {
        throw new NotImplementedException();
    }
}
