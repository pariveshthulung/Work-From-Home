using System;
using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Request.Queries;

public class GetEmployeeByEmailQuery : IRequest<BaseResult<EmployeeDto>>
{
    public string Email { get; set; }
}
