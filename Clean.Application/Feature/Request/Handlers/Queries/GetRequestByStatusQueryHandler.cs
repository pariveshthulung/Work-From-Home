using System;
using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Request.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Handlers.Queries;

public class GetRequestByStatusQueryHandler
    : IRequestHandler<GetRequestByStatusQuery, BaseResult<List<RequestDto>>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetRequestByStatusQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<RequestDto>>> Handle(
        GetRequestByStatusQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employees = await _employeeRepository.GetAllEmployeeAsync(
                request.Query.SearchTerm,
                request.Query.SortColumn,
                request.Query.SortOrder,
                request.Query.PageNumber,
                request.Query.PageSize,
                cancellationToken
            );
            var requests = employees
                .Items.SelectMany(x => x.Requests)
                .Where(x => x.RequestedTypeId == request.StatusId)
                .ToList();
            return BaseResult<List<RequestDto>>.Ok(_mapper.Map<List<RequestDto>>(requests));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
