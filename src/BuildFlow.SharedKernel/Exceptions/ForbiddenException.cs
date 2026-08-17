namespace BuildFlow.SharedKernel.Exceptions;

/// <summary>
/// Thrown when a user attempts to access a resource outside their tenant boundary,
/// or lacks the required permission. Mapped to HTTP 403 by Global Exception Middleware.
/// </summary>
public sealed class ForbiddenException : AppException
{
    public ForbiddenException()
        : base("You do not have permission to perform this action.")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }
}
