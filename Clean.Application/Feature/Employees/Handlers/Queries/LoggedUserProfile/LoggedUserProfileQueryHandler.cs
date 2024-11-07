using System;
using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Commands.LoggedUserProfile;

public class LoggedUserProfileQueryHandler
    : IRequestHandler<LoggedUserProfileQuery, BaseResult<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public LoggedUserProfileQueryHandler(
        IEmployeeRepository employeeRepository,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _employeeRepo = employeeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResult<EmployeeDto>> Handle(
        LoggedUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        var employee = await _employeeRepo.GetEmployeeByEmailAsync(
            _currentUserService.UserEmail,
            cancellationToken
        );
        if (employee is null)
            return BaseResult<EmployeeDto>.Failure(EmployeeErrors.Unauthorize());
        return BaseResult<EmployeeDto>.Ok(_mapper.Map<EmployeeDto>(employee));
    }
}
