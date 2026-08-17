namespace BuildFlow.SharedKernel.Events;

/// <summary>
/// Base record for all domain events. Provides a default EventId and OccurredOn timestamp.
/// Derive concrete events from this record:
///   public sealed record ProjectCreatedEvent(Guid ProjectId) : DomainEvent;
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
