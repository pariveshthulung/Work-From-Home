using System;
using Clean.Application.Dto.Employee;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Requests.Queries;

public class GetEmployeeQuery : IRequest<BaseResult<EmployeeDto>>
{
    public int Id { get; set; }
    public Guid GuidId { get; set; }
}
