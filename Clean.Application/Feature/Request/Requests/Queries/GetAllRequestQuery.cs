using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Requests.Queries;

public class GetAllRequestQuery : IRequest<BaseResult<List<RequestDto>>>
{
    public QueryObject Query { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
