using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Requests.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Handlers.Queries;

public class GetRequestQueryHandler : IRequestHandler<GetRequestQuery, BaseResult<RequestDto>>
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepo;

    public GetRequestQueryHandler(IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _mapper = mapper;
        _employeeRepo = employeeRepository;
    }

    public async Task<BaseResult<RequestDto>> Handle(
        GetRequestQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employees = await _employeeRepo.GetEmployeeByGuidIdAsync(
                query.EmployeeId,
                cancellationToken
            );
            if (employees is null)
                return BaseResult<RequestDto>.Failure(EmployeeErrors.NotFound());

            var request = employees.Requests.FirstOrDefault(x => x.GuidId == query.RequestId);
            if (request is null)
                return BaseResult<RequestDto>.Failure(RequestErrors.NotFound());
            return BaseResult<RequestDto>.Ok(_mapper.Map<RequestDto>(request));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
