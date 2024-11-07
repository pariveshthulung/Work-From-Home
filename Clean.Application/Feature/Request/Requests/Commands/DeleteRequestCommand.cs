using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Requests.Commands;

public class DeleteRequestCommand : IRequest<BaseResult<Unit>>
{
    public Guid RequestId { get; set; }
    public Guid EmployeeId { get; set; }
}
