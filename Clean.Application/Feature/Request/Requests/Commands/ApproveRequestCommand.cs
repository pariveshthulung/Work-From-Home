using Clean.Application.Dto.Approval;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Requests.Requests.Commands;

public class ApproveRequestCommand : IRequest<BaseResult<int>>
{
    public ApproveRequestDto ApproveRequestDto { get; set; }
}
