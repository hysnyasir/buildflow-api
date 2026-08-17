namespace BuildFlow.SharedKernel.Exceptions;

/// <summary>
/// Thrown when input validation fails.
/// Mapped to HTTP 400 by Global Exception Middleware.
/// </summary>
public sealed class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string field, string message)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, [message] }
        };
    }
}
