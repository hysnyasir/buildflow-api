namespace BuildFlow.SharedKernel.Primitives;

/// <summary>
/// Base class for all domain entities. Provides a strongly-typed Guid identifier.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }
}
