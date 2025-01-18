using System;
using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Feature.Employees.Request.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries;

public class GetEmployeeByEmailQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<GetEmployeeByEmailQuery, BaseResult<EmployeeDto>>
{
    public async Task<BaseResult<EmployeeDto>> Handle(
        GetEmployeeByEmailQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employee = await employeeRepository.GetEmployeeByEmailAsync(
                request.Email,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<EmployeeDto>.Failure(EmployeeErrors.NotFound());
            return BaseResult<EmployeeDto>.Ok(mapper.Map<EmployeeDto>(employee));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
