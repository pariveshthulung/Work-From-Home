using System.Net;
using Clean.Api.Controller.Common;
using Clean.Api.Extension;
using Clean.Application.Dto.Token;
using Clean.Application.Feature.Auth.Commands.ChangePassword;
using Clean.Application.Feature.Auth.Commands.Login;
using Clean.Application.Feature.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "okta,custom")]
    public class AuthController(IMediator mediator) : BaseController
    {
        [HttpPost("registerUser")]
        [Authorize(Policy = "Adminstrator")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterUserAsync(
            [FromBody] RegisterCommand command,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(command, cancellationToken);

            if (!response.Success)
                return response.ToProblemDetail();

            return Created();
        }

        [HttpPost("loginUser")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Tokens), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync(
            LoginCommand command,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPut("changepassword")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordCommand command,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(command, cancellationToken);
            if (!response.Success)
                return response.ToProblemDetail();

            return NoContent();
        }
    }
}
