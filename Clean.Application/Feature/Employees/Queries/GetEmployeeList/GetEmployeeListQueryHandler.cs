using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Feature.Employees.Requests.Queries;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Enums;
using MediatR;

namespace Clean.Application.Feature.Employees.Handlers.Queries;

public class GetEmployeeListQueryHandler
    : IRequestHandler<GetEmployeeListQuery, BaseResult<PagedList<EmployeeDto>>>
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetEmployeeListQueryHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        ICurrentUserService currentUserService
    )
    {
        _employeeRepo = employeeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResult<PagedList<EmployeeDto>>> Handle(
        GetEmployeeListQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var currentUser = await _employeeRepo.GetEmployeeByEmailAsync(
                _currentUserService.UserEmail!,
                cancellationToken
            );
            var employees = await _employeeRepo.GetAllEmployeeAsync(
                currentUser!.Id,
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            return BaseResult<PagedList<EmployeeDto>>.Ok(employees);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
