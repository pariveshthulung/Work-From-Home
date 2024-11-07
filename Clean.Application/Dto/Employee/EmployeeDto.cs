using Clean.Application.Dto.Address;
using Clean.Application.Dto.Base;
using Clean.Application.Dto.Request;

namespace Clean.Application.Dto.Employee;

public record EmployeeDto : BaseDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public int UserRoleId { get; init; }
    public string UserRole { get; init; }
    public string PhoneNumber { get; init; }
    public AddressDto Address { get; init; }
    public List<RequestDto> Requests { get; init; }
}
