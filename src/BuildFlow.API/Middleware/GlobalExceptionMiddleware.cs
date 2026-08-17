using BuildFlow.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BuildFlow.API.Middleware;

/// <summary>
/// Catches all unhandled exceptions and returns RFC 7807 ProblemDetails responses.
/// Maps each AppException subtype to the correct HTTP status code.
/// Stack traces are never exposed — safe for production.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception on {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, extensions) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        if (extensions is not null)
        {
            foreach (var (key, value) in extensions)
            {
                problemDetails.Extensions[key] = value;
            }
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }

    private static (int StatusCode, string Title, Dictionary<string, object?>? Extensions)
        MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (
                (int)HttpStatusCode.NotFound,
                "Resource Not Found",
                null),

            ValidationException validationEx => (
                (int)HttpStatusCode.BadRequest,
                "Validation Failed",
                new Dictionary<string, object?> { ["errors"] = validationEx.Errors }),

            ForbiddenException => (
                (int)HttpStatusCode.Forbidden,
                "Forbidden",
                null),

            ConflictException => (
                (int)HttpStatusCode.Conflict,
                "Conflict",
                null),

            AppException => (
                (int)HttpStatusCode.BadRequest,
                "Bad Request",
                null),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred.",
                null)
        };
    }
}
