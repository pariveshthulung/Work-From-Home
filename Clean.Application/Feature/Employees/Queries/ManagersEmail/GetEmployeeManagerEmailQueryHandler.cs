using System;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries.ManagersEmail;

public class GetManagersEmailQueryHandler
    : IRequestHandler<GetEmployeeManagerEmailQuery, BaseResult<string>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetManagersEmailQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<BaseResult<string>> Handle(
        GetEmployeeManagerEmailQuery request,
        CancellationToken cancellationToken
    )
    {
        var emails = await _employeeRepository.GetEmployeeManagerEmailAsync(
            request.Email,
            cancellationToken
        );
        return BaseResult<string>.Ok(emails);
    }
}
