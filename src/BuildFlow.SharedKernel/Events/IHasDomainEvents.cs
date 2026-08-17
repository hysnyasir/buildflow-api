namespace BuildFlow.SharedKernel.Events;

/// <summary>
/// Applied to aggregate roots that raise domain events.
/// The persistence layer reads and dispatches these events after SaveChanges.
/// </summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
