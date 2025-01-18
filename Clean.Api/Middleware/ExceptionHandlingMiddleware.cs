using System;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occured: {message}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = (int)StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://httpstatuses.com/500",
                Extensions = new Dictionary<string, object?>
                {
                    {"errors",new {fieldName = "Error" , descriptions ="An unexpected error occurred. Please try again later."}}
                }
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
