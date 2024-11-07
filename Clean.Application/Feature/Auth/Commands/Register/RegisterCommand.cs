using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Auth.Commands.Register;

public class RegisterCommand : IRequest<BaseResult<int>>
{
    public RegisterEmployeeDto RegisterEmployeeDto { get; set; }
}
