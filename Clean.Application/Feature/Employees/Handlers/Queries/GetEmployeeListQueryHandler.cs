using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Feature.Employees.Requests.Queries;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries;

public class GetEmployeeListQueryHandler
    : IRequestHandler<GetEmployeeListQuery, BaseResult<PagedList<EmployeeDto>>>
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IMapper _mapper;

    public GetEmployeeListQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepo = employeeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<PagedList<EmployeeDto>>> Handle(
        GetEmployeeListQuery request,
        CancellationToken cancellationToken
    )
    {
        var employees = await _employeeRepo.GetAllEmployeeAsync(
            request.SearchTerm,
            request.SortColumn,
            request.SortOrder,
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
        return BaseResult<PagedList<EmployeeDto>>.Ok(employees);
    }
}
