using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildFlow.Application.Common.Behaviours;

/// <summary>
/// MediatR pipeline behaviour that logs the start and completion
/// of every request. Uses structured logging — no string interpolation.
/// </summary>
public sealed class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("BuildFlow — Handling {RequestName}", requestName);

        var response = await next(cancellationToken);

        _logger.LogInformation("BuildFlow — Handled {RequestName}", requestName);

        return response;
    }
}
