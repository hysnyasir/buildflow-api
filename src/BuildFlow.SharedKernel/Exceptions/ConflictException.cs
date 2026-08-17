namespace BuildFlow.SharedKernel.Exceptions;

/// <summary>
/// Thrown when a resource already exists and a duplicate would be created.
/// Mapped to HTTP 409 by Global Exception Middleware.
/// </summary>
public sealed class ConflictException : AppException
{
    public ConflictException(string entityName, string detail)
        : base($"{entityName} already exists: {detail}")
    {
    }

    public ConflictException(string message) : base(message)
    {
    }
}
