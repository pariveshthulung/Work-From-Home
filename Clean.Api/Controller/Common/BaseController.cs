using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controller.Common
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string UserEmail => User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }
}
