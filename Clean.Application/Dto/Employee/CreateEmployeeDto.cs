using Clean.Application.Dto.Address;

namespace Clean.Application.Dto.Employee;

public record CreateEmployeeDto
{
    // public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public int UserRoleId { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
}
