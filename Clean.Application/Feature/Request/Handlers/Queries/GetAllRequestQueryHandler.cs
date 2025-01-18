using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Requests.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Enums;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Queries;

public class GetAllRequestQueryHandler
    : IRequestHandler<GetAllRequestQuery, BaseResult<List<RequestDto>>>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    private readonly IEmployeeRepository _employeeRepo;

    public GetAllRequestQueryHandler(
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _employeeRepo = employeeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResult<List<RequestDto>>> Handle(
        GetAllRequestQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employees = await _employeeRepo.GetAllEmployeeAsync(
                query.Query.SearchTerm,
                query.Query.SortColumn,
                query.Query.SortOrder,
                query.Query.PageNumber,
                query.Query.PageSize,
                cancellationToken
            );
            var currentUser = await _employeeRepo.GetEmployeeByEmailAsync(
                _currentUserService.UserEmail!,
                cancellationToken
            );
            if (
                currentUser?.UserRoleId == UserRoleEnum.Admin.Id
                || currentUser?.UserRoleId == UserRoleEnum.SuperAdmin.Id
            )
            {
                var requests = employees.Items?.SelectMany(x => x.Requests).ToList();
                return BaseResult<List<RequestDto>>.Ok(requests);
            }
            else
            {
                var requests = employees
                    .Items?.Where(x => x.ManagerId == currentUser!.Id)
                    .SelectMany(x => x.Requests)
                    .ToList();
                return BaseResult<List<RequestDto>>.Ok(_mapper.Map<List<RequestDto>>(requests));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
