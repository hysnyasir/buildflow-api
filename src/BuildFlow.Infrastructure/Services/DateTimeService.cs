using BuildFlow.Domain.Interfaces;

namespace BuildFlow.Infrastructure.Services;

/// <summary>
/// Concrete implementation of IDateTimeService.
/// Wraps DateTimeOffset.UtcNow so it can be mocked in unit tests.
/// Registered as Singleton — stateless, safe to share across requests.
/// </summary>
public sealed class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
