using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Request.Commands;

public class DeleteEmployeeCommand : IRequest<BaseResult<Unit>>
{
    public Guid EmployeeId { get; set; }
}
