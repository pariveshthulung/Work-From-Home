using Clean.Application.Dto.Address;
using Clean.Application.Dto.Base;
using Clean.Application.Dto.Request;

namespace Clean.Application.Dto.Employee;

public record EmployeeDto : BaseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public int UserRoleId { get; init; }
    public string UserRole { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
    public List<RequestDto> Requests { get; init; } = default!;
}
