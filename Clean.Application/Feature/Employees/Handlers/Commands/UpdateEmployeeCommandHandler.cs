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
        var validator = new UpdateEmployeeDtoValitator();
        var validatorResult = await validator.ValidateAsync(
            request.UpdateEmployeeDto,
            cancellationToken
        );
        if (!validatorResult.IsValid)
        {
            var errors = validatorResult
                .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                .ToList();
            return BaseResult<Unit>.Failure(errors);
        }
        var employee = await _employeeRepo.GetEmployeeByGuidIdAsync(
            request.UpdateEmployeeDto.Id,
            cancellationToken
        );

        if (employee is null)
            return BaseResult<Unit>.Failure(EmployeeErrors.NotFound());

        var currentUser = await _employeeRepo.GetEmployeeByEmailAsync(
            _currentUser.UserEmail,
            cancellationToken
        );

        employee.Update(
            request.UpdateEmployeeDto.Name,
            request.UpdateEmployeeDto.Email,
            request.UpdateEmployeeDto.PhoneNumber,
            request.UpdateEmployeeDto.UserRoleId,
            _mapper.Map<Address>(request.UpdateEmployeeDto.Address) // employee.Address!.ToAddress()
        );
        employee.SetUpdatedBy(currentUser!.Id);

        await _employeeRepo.UpdateEmployeeAsync(employee, cancellationToken);
        return BaseResult<Unit>.Ok(Unit.Value);
    }
}
