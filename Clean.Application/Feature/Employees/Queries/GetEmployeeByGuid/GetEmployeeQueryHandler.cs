using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Exceptions;
using Clean.Application.Feature.Employees.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries;

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, BaseResult<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IMapper _mapper;

    public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepo = employeeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<EmployeeDto>> Handle(
        GetEmployeeQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employee = await _employeeRepo.GetEmployeeByGuidIdAsync(
                request.GuidId,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<EmployeeDto>.Failure(EmployeeErrors.NotFound());
            return BaseResult<EmployeeDto>.Ok(_mapper.Map<EmployeeDto>(employee));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
