using Clean.Application.Feature.Employees.Request.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Commands;

public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<DeleteEmployeeCommand, BaseResult<Unit>>
{
    public async Task<BaseResult<Unit>> Handle(
        DeleteEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employee = await employeeRepository.GetEmployeeByGuidIdAsync(
                command.EmployeeId,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<Unit>.Failure(EmployeeErrors.NotFound());
            if (employee.AppUser is null)
                return BaseResult<Unit>.Failure(
                    new Error(400, "AppUser.Null", "AppUser doesnot exist.")
                );

            employee.AppUser.SetIsDeleted(true);
            employee.SetIsDeleted(true);

            await employeeRepository.DeleteEmployeeAsync(employee, cancellationToken);
            return BaseResult<Unit>.Ok(Unit.Value);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
