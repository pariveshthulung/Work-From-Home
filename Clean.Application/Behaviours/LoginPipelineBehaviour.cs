using System;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clean.Application.Behaviours;

public class LoginPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResult
{
    private readonly ILogger<LoginPipelineBehaviour<TRequest, TResponse>> _logger;

    public LoginPipelineBehaviour(ILogger<LoginPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Handling Request : {@RequestName} , {@DateTimeNow}",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );
        var result = await next();
        if (!result.Success)
        {
            _logger.LogError(
                "Errors: {@RequestName} {@Error} , {@DateTimeNow}",
                typeof(TRequest).Name,
                result.Errors,
                DateTime.UtcNow
            );
        }
        _logger.LogInformation(
            "Handled Request: {@RequestName} , {@DateTimeNow} ",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );
        return result;
    }
}
