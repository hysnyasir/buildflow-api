namespace BuildFlow.Domain.Interfaces;

/// <summary>
/// Abstracts DateTimeOffset.UtcNow to make time-dependent code testable.
/// Inject this instead of calling DateTimeOffset.UtcNow directly.
/// </summary>
public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}
