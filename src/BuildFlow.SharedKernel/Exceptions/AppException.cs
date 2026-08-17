namespace BuildFlow.SharedKernel.Exceptions;

/// <summary>
/// Base exception for all application-level exceptions in BuildFlow.
/// Caught by Global Exception Middleware and mapped to ProblemDetails.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }

    protected AppException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
