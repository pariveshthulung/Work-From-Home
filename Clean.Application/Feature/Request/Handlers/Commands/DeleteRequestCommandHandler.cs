using AutoMapper;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Commands;

public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, BaseResult<Unit>>
{
    private readonly IEmployeeRepository _employeeRepo;

    public DeleteRequestCommandHandler(IEmployeeRepository employeeRepo)
    {
        _employeeRepo = employeeRepo;
    }

    public async Task<BaseResult<Unit>> Handle(
        DeleteRequestCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employee = await _employeeRepo.GetEmployeeByGuidIdAsync(
                command.EmployeeId,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<Unit>.Failure(EmployeeErrors.NotFound());
            var existingEmployee = employee;
            var request = employee.Requests.FirstOrDefault(x => x.GuidId == command.RequestId);
            if (request is null)
                return BaseResult<Unit>.Failure(RequestErrors.NotFound());
            employee.DeleteRequest(request);

            await _employeeRepo.UpdateEmployeeAsync(employee, existingEmployee, cancellationToken);
            return BaseResult<Unit>.Ok(Unit.Value);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
