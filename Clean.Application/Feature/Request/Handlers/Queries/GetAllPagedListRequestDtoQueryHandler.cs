using System;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Request.Requests.Queries;
using Clean.Application.Helper;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Handlers.Queries;

public class GetAllPagedListRequestDtoQueryHandler
    : IRequestHandler<GetAllPagedListRequestDtoQuery, BaseResult<PagedList<RequestDto>>>
{
    private readonly IRequestRepository _requestRepository;

    public GetAllPagedListRequestDtoQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<BaseResult<PagedList<RequestDto>>> Handle(
        GetAllPagedListRequestDtoQuery request,
        CancellationToken cancellationToken
    )
    {
        var requestDto = await _requestRepository.GetAllRequestAsync(
            request.SearchTerm,
            request.SortColumn,
            request.SortOrder,
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
        return BaseResult<PagedList<RequestDto>>.Ok(requestDto);
    }
}
