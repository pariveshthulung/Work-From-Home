using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Requests.Commands;

public class RegisterEmployeeCommand : IRequest<BaseResult<int>>
{
    public RegisterEmployeeDto RegisterEmployeeDto { get; set; }
}
