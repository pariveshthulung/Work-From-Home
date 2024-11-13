using System;
using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Requests.Queries;

public class GetAllPagedListRequestDtoQuery : IRequest<BaseResult<PagedList<RequestDto>>>
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
