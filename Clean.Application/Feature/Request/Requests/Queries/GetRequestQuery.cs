using Clean.Application.Dto.Request;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Requests.Queries;

public class GetRequestQuery : IRequest<BaseResult<RequestDto>>
{
    public Guid RequestId { get; init; }
    public Guid EmployeeId { get; init; }
}
