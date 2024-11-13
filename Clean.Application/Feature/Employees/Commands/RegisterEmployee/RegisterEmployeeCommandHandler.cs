using AutoMapper;
using Clean.Application.Dto.Employee.Validation;
using Clean.Application.Feature.Employees.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Clean.Application.Feature.Employees.Handlers.Commands;

public class RegisterEmployeeCommandHandler
    : IRequestHandler<RegisterEmployeeCommand, BaseResult<int>>
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly RoleManager<UserRole> _roleManager;

    public RegisterEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        RoleManager<UserRole> roleManager
    )
    {
        _mapper = mapper;
        _employeeRepo = employeeRepository;
        _roleManager = roleManager;
    }

    public async Task<BaseResult<int>> Handle(
        RegisterEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        var validator = new RegisterEmployeeDtoValidator(_employeeRepo);

        var validatorResult = await validator.ValidateAsync(
            request.RegisterEmployeeDto,
            cancellationToken
        );
        if (!validatorResult.IsValid)
        {
            var errors = validatorResult
                .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                .ToList();
            return BaseResult<int>.Failure(errors);
        }
        var checkEmail = await _employeeRepo.EmailExistAsync(
            request.RegisterEmployeeDto.Email,
            cancellationToken
        );
        if (checkEmail)
            return BaseResult<int>.Failure(
                EmployeeErrors.EmailNotUnique(request.RegisterEmployeeDto.Email)
            );

        var employee = _mapper.Map<Employee>(request.RegisterEmployeeDto);
        employee = await _employeeRepo.AddEmployeeAsync(employee, cancellationToken);

        return BaseResult<int>.Ok(employee!.Id);
    }
}
