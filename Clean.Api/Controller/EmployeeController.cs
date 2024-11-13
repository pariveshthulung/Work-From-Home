using System.Net;
using Clean.Api.Controller.Common;
using Clean.Api.Extension;
using Clean.Application.Dto.Employee;
using Clean.Application.Feature.Employees.Handlers.Commands.LoggedUserProfile;
using Clean.Application.Feature.Employees.Handlers.Queries.ManagersEmail;
using Clean.Application.Feature.Employees.Request.Commands;
using Clean.Application.Feature.Employees.Request.Queries;
using Clean.Application.Feature.Employees.Requests.Commands;
using Clean.Application.Feature.Employees.Requests.Queries;
using Clean.Application.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controller
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "okta,custom")]
    public class EmployeeController(IMediator mediator) : BaseController
    {
        #region GetMethod
        // [Authorize(Policy = "Adminstrator")]
        [HttpGet("getAllEmployee")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedList<EmployeeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllEmployeeAsync(
            string? searchTerm,
            string? sortOrder,
            string? sortColumn,
            int page = 1,
            int pageSize = 25,
            CancellationToken cancellationToken = default
        )
        {
            var response = await mediator.Send(
                new GetEmployeeListQuery
                {
                    SearchTerm = searchTerm,
                    SortColumn = sortColumn,
                    SortOrder = sortOrder,
                    PageNumber = page,
                    PageSize = pageSize,
                },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        // [HttpGet("{email}", Name = "GetEmployeeByEmail")]
        [HttpGet("GetEmployeeEmail")]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEmployeeByEmailAsync(
            string email,
            CancellationToken cancellationToken
        )
        {
            var request = new GetEmployeeByEmailQuery { Email = email };
            var response = await mediator.Send(request, cancellationToken);

            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet("GetLoggedInUser")]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLoggedInUserAsync(CancellationToken cancellationToken)
        {
            try
            {
                var request = new LoggedUserProfileQuery();
                var response = await mediator.Send(request, cancellationToken);

                if (!response.Success)
                    return response.ToProblemDetail();

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getEmployeeByGuidId")]
        [ProducesResponseType(typeof(EmployeeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEmployeeByGuidIdAsync(
            Guid guidId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new GetEmployeeQuery { GuidId = guidId },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }
        #endregion

        [HttpPut("updateEmployee")]
        [Authorize(Policy = "Adminstrator")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateEmployeeAsync(
            [FromBody] UpdateEmployeeDto updateEmployeeDto,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new UpdateEmployeeCommand { UpdateEmployeeDto = updateEmployeeDto },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return NoContent();
            // return Results.Ok(response.Data);
        }

        [Authorize(Policy = "Adminstrator")]
        [HttpDelete("deleteEmployee")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEmployeeAsync(
            Guid employeeId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(
                new DeleteEmployeeCommand { EmployeeId = employeeId },
                cancellationToken
            );
            if (!response.Success)
                return response.ToProblemDetail();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("getManagersEmail")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetManagersEmail(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetManagersEmailQuery(), cancellationToken);
            if (!response.Success)
                return response.ToProblemDetail();

            return Ok(response.Data);
        }
    }
}
