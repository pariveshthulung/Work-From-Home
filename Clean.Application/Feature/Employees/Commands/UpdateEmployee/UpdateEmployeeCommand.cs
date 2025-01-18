using System;
using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Requests.Commands;

public class UpdateEmployeeCommand : IRequest<BaseResult<Unit>>
{
    public UpdateEmployeeDto UpdateEmployeeDto { get; set; }
}
