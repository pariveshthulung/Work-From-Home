using Clean.Application.Feature.ApprovalStatusEnums.Queries;
using Clean.Application.Feature.RequestTypeEnum.Queries;
using Clean.Application.Feature.RoleEnum.Queries;
using Clean.Application.Persistence.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controller
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        private readonly IEnumRepository _enumRepo;
        private readonly IMediator _mediator;

        public EnumController(IEnumRepository enumRepository, IMediator mediator)
        {
            _enumRepo = enumRepository;
            _mediator = mediator;
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoleAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserRolesQuery(), cancellationToken);
            return Ok(result.Data);
        }

        [HttpGet("getRequestedType")]
        public async Task<IActionResult> GetRequestedTypeAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RequestTypeEnumQuery(), cancellationToken);
            return Ok(result.Data);
        }

        [HttpGet("getApprovalStatus")]
        public async Task<IActionResult> GetApprovalStatusAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ApproveStatusEnumQuery(), cancellationToken);
            return Ok(result.Data);
        }
    }
}
