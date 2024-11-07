using System;
using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Requests.Queries;

public class GetRequestByStatusQuery : IRequest<BaseResult<List<RequestDto>>>
{
    public QueryObject Query { get; set; }
    public int StatusId { get; set; }
}
