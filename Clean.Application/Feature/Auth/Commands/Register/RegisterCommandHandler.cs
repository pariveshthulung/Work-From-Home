using System;
using System.Transactions;
using AutoMapper;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using Clean.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Clean.Application.Feature.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResult<int>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public RegisterCommandHandler(
        IEmployeeRepository employeeRepository,
        UserManager<AppUser> userManager,
        RoleManager<UserRole> roleManager,
        IMapper mapper,
        ICurrentUserService currentUserService
    )
    {
        _employeeRepository = employeeRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResult<int>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var validator = new RegisterCommandValidator();
            var result = await validator.ValidateAsync(
                request.RegisterEmployeeDto,
                cancellationToken
            );
            if (!result.IsValid)
            {
                var errors = result
                    .Errors.Select(e => new Error(400, e.PropertyName, e.ErrorMessage))
                    .ToList();
                return BaseResult<int>.Failure(errors);
            }
            var emailExist = await _employeeRepository.EmailExistIncludeDeletedAsync(
                request.RegisterEmployeeDto.Email,
                cancellationToken
            );
            if (emailExist)
                return BaseResult<int>.Failure(
                    EmployeeErrors.EmailNotUnique(request.RegisterEmployeeDto.Email)
                );
            var currentUser = await _employeeRepository.GetEmployeeByEmailAsync(
                _currentUserService.UserEmail!,
                cancellationToken
            );
            if (
                !CheckRegisterPermission(
                    currentUser!.UserRoleId,
                    request.RegisterEmployeeDto.UserRoleId
                )
            )
                return BaseResult<int>.Failure(
                    new Error(
                        403,
                        "Forbidden",
                        $"You dont have permission to add new employee as {UserRoleEnum.FromId(request.RegisterEmployeeDto.UserRoleId).Name}"
                    )
                );
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            if (
                !await _roleManager.RoleExistsAsync(
                    UserRoleEnum.FromId(request.RegisterEmployeeDto.UserRoleId).Name
                )
            )
                return BaseResult<int>.Failure(
                    new Error(
                        400,
                        "Role",
                        $"Role with id '{request.RegisterEmployeeDto.UserRoleId}' was not found."
                    )
                );

            var employee = _mapper.Map<GeneralEmployee>(request.RegisterEmployeeDto);
            var appUser = new AppUser { UserName = employee.Name, Email = employee.Email, };

            var validationResult = await _userManager.CreateAsync(
                appUser,
                request.RegisterEmployeeDto.Password
            );

            if (!validationResult.Succeeded)
            {
                var errors = validationResult
                    .Errors.Select(e => new Error(400, e.Code, e.Description))
                    .ToList();
                return BaseResult<int>.Failure(errors);
            }

            var role = UserRoleEnum.FromId(request.RegisterEmployeeDto.UserRoleId).Name;
            await _userManager.AddToRoleAsync(appUser, role);

            employee.SetAddedBy(currentUser!.Id);
            employee.SetAddedOn(DateTime.UtcNow);
            employee.SetAppUserId(appUser.Id);
            employee.SetManagerId(currentUser.Id);
            appUser.SetPasswordExpire(true);

            await _employeeRepository.AddEmployeeAsync(employee, cancellationToken);

            transaction.Complete();

            return BaseResult<int>.Ok(employee.Id);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public bool CheckRegisterPermission(int currentUserRole, int newUserRole)
    {
        if (currentUserRole == UserRoleEnum.SuperAdmin.Id)
        {
            return true;
        }
        if (currentUserRole == UserRoleEnum.Admin.Id)
            if (newUserRole == UserRoleEnum.SuperAdmin.Id || newUserRole == UserRoleEnum.Admin.Id)
            {
                return false;
            }
        if (currentUserRole == UserRoleEnum.Manager.Id)
            if (
                newUserRole == UserRoleEnum.SuperAdmin.Id
                || newUserRole == UserRoleEnum.Admin.Id
                || newUserRole == UserRoleEnum.Manager.Id
            )
            {
                return false;
            }

        return true;
    }
}
