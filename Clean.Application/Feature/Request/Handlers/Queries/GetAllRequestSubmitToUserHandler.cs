using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Request.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Enums;
using MediatR;

namespace Clean.Application.Feature.Request.Handlers.Queries;

public class GetAllRequestSubmitToUserHandler
    : IRequestHandler<GetAllRequestSubmitToUser, BaseResult<List<RequestDto>>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetAllRequestSubmitToUserHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<RequestDto>>> Handle(
        GetAllRequestSubmitToUser query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employees = await _employeeRepository.GetAllEmployeeAsync(
                query.Query.SearchTerm,
                query.Query.SortColumn,
                query.Query.SortOrder,
                query.Query.PageNumber,
                query.Query.PageSize,
                cancellationToken
            );
            var employee = await _employeeRepository.GetEmployeeByEmailAsync(
                query.Email,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<List<RequestDto>>.Failure(RequestErrors.NotFound());
            var requests = employees
                .Items.SelectMany(x => x.Requests)
                .Where(x =>
                    x.RequestedTo == employee.Id
                    && x.Approval.ApprovalStatusId == ApprovalStatusEnum.Pending.Id
                )
                .ToList();
            return BaseResult<List<RequestDto>>.Ok(_mapper.Map<List<RequestDto>>(requests));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
