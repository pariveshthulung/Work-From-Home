using System;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries.ManagersEmail;

public class GetManagersEmailQueryHandler
    : IRequestHandler<GetManagersEmailQuery, BaseResult<List<string>>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetManagersEmailQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<BaseResult<List<string>>> Handle(
        GetManagersEmailQuery request,
        CancellationToken cancellationToken
    )
    {
        var emails = await _employeeRepository.GetManagerEmailAsync(cancellationToken);
        return BaseResult<List<string>>.Ok(emails);
    }
}
