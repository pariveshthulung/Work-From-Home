using Clean.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Extension;

public static class ResultExtension
{
    public static IActionResult ToProblemDetail<T>(this BaseResult<T> result)
    {
        if (result.Success)
        {
            return new OkResult();
        }

        // Use the ErrorCode from the first error as the status code
        var statusCode =
            result.Errors?.FirstOrDefault()?.ErrorCode ?? StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = result?.Errors?.FirstOrDefault()?.FieldName ?? "Error:",
            Type = $"https://httpstatuses.com/{statusCode}",
            Extensions = new Dictionary<string, object?> { { "errors", result?.Errors?.ToList() } }
        };

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }
}
