using System;
using Clean.Application.Dto.Address;

namespace Clean.Application.Dto.Employee;

public interface IEmployeeDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public int UserRoleId { get; init; }
    public string PhoneNumber { get; init; }
    public AddressDto Address { get; init; }
}
