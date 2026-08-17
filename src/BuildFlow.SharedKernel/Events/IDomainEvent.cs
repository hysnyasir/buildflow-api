using MediatR;

namespace BuildFlow.SharedKernel.Events;

/// <summary>
/// Marker interface for domain events.
/// Implements MediatR INotification so events can be dispatched
/// via the MediatR pipeline after SaveChanges.
/// </summary>
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}
