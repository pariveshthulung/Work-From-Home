using System;
using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Requests.Queries;

public class GetAllUserRequestQuery : IRequest<BaseResult<List<RequestDto>>>
{
    public string Email { get; set; }
    public QueryObject Query { get; set; }
}
