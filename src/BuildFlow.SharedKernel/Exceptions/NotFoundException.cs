namespace BuildFlow.SharedKernel.Exceptions;

/// <summary>
/// Thrown when a requested resource does not exist.
/// Mapped to HTTP 404 by Global Exception Middleware.
/// </summary>
public sealed class NotFoundException : AppException
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with identifier '{key}' was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}
