using Clean.Application.Dto.Request;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Requests.Commands;

public class SubmitRequestCommand : IRequest<BaseResult<Guid>>
{
    public CreateRequestDto CreateRequestDto { get; set; }
}
