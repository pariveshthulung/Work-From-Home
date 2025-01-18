using AutoMapper;
using Clean.Application.Dto.Employee.Validation;
using Clean.Application.Feature.Employees.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Commands;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, BaseResult<Unit>>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IEmployeeRepository _employeeRepo;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        ICurrentUserService userService
    )
    {
        _mapper = mapper;
        _currentUser = userService;
        _employeeRepo = employeeRepository;
    }

    public async Task<BaseResult<Unit>> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var validator = new UpdateEmployeeDtoValitator();
            var validatorResult = await validator.ValidateAsync(
                request.UpdateEmployeeDto,
                cancellationToken
            );
            if (!validatorResult.IsValid)
            {
                var errors = validatorResult
                    .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage.ToString()))
                    .ToList();
                return BaseResult<Unit>.Failure(errors);
            }
            var employee = await _employeeRepo.GetEmployeeByGuidIdAsync(
                request.UpdateEmployeeDto.Id,
                cancellationToken
            );
            var existingEmployee = employee;

            if (employee is null)
                return BaseResult<Unit>.Failure(EmployeeErrors.NotFound());

            var currentUser = await _employeeRepo.GetEmployeeByEmailAsync(
                _currentUser.UserEmail,
                cancellationToken
            );

            var addressDetails = _mapper.Map<Address>(request.UpdateEmployeeDto.Address);
            employee.Update(
                request.UpdateEmployeeDto.Name,
                request.UpdateEmployeeDto.Email,
                request.UpdateEmployeeDto.PhoneNumber,
                request.UpdateEmployeeDto.UserRoleId
            );

            employee.UpdateEmployeeAddress(addressDetails);
            employee.SetUpdatedBy(currentUser.Id);

            await _employeeRepo.UpdateEmployeeAsync(employee, existingEmployee, cancellationToken);
            return BaseResult<Unit>.Ok(Unit.Value);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
