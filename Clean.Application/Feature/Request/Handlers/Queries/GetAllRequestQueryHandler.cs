using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Requests.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Queries;

public class GetAllRequestQueryHandler
    : IRequestHandler<GetAllRequestQuery, BaseResult<List<RequestDto>>>
{
    private readonly IMapper _mapper;

    private readonly IEmployeeRepository _employeeRepo;

    public GetAllRequestQueryHandler(IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _mapper = mapper;
        _employeeRepo = employeeRepository;
    }

    public async Task<BaseResult<List<RequestDto>>> Handle(
        GetAllRequestQuery query,
        CancellationToken cancellationToken
    )
    {
        var employees = await _employeeRepo.GetAllEmployeeAsync(
            query.Query.SearchTerm,
            query.Query.SortColumn,
            query.Query.SortOrder,
            query.Query.PageNumber,
            query.Query.PageSize,
            cancellationToken
        );
        var requests = employees.Items?.SelectMany(x => x.Requests).ToList();

        return BaseResult<List<RequestDto>>.Ok(_mapper.Map<List<RequestDto>>(requests));
    }
}
