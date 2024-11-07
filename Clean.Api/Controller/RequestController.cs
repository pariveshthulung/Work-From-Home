using System.Net;
using Clean.Api.Controller.Common;
using Clean.Api.Extension;
using Clean.Application.Dto.Approval;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Request.Requests.Queries;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Feature.Requests.Requests.Queries;
using Clean.Application.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controller
{
    [Authorize(AuthenticationSchemes = "okta,custom")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class RequestController(IMediator mediator) : BaseController
    {
        [Authorize(Policy = "Adminstrator")]
        [HttpGet("getAllRequest")]
        [ProducesResponseType(typeof(List<RequestDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllRequestAsync(
            [FromQuery] QueryObject query,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new GetAllRequestQuery { Query = query },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [Authorize(Policy = "Adminstrator")]
        [HttpGet("getAllRequestSubmitToUser")]
        [ProducesResponseType(typeof(List<RequestDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> getAllRequestSubmitToUserAsync(
            string email,
            [FromQuery] QueryObject query,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new GetAllRequestSubmitToUser { Query = query, Email = email },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [HttpGet("getRequestByGuidId")]
        [ProducesResponseType(typeof(RequestDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRequestByGuidIdAsync(
            Guid requestId,
            Guid employeeId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new GetRequestQuery { EmployeeId = employeeId, RequestId = requestId },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [HttpGet("getAllUserRequestQuery")]
        [ProducesResponseType(typeof(RequestDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllUserRequestQueryAsync(
            string email,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new GetAllUserRequestQuery { Email = email },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [HttpDelete("deleteRequest")]
        [Authorize(Policy = "Adminstrator")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteRequestAsync(
            Guid employeeId,
            Guid requestId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new DeleteRequestCommand { EmployeeId = employeeId, RequestId = requestId },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return NoContent();
        }

        [HttpPost("submitrequest")]
        [Authorize]
        [ProducesResponseType(typeof(RequestDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SubmitRequestAsync(
            [FromBody] CreateRequestDto createRequestDto,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new SubmitRequestCommand { CreateRequestDto = createRequestDto },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();
            return Created();
        }

        [HttpPut("approverequest")]
        [Authorize(Policy = "Adminstrator")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ApproveRequestAsync(
            [FromBody] ApproveRequestDto approveRequestDto,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new ApproveRequestCommand { ApproveRequestDto = approveRequestDto },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return NoContent();
        }
    }
}
