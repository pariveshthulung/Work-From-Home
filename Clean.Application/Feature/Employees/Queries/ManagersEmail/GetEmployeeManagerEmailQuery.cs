using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries.ManagersEmail;

public class GetEmployeeManagerEmailQuery : IRequest<BaseResult<string>>
{
    public string Email { get; set; } = default!;
}
