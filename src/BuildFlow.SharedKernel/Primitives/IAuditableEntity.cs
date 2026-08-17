namespace BuildFlow.SharedKernel.Primitives;

/// <summary>
/// Marks an entity as auditable. Used by the persistence layer to
/// automatically set audit fields during SaveChanges.
/// </summary>
public interface IAuditableEntity
{
    Guid TenantId { get; set; }
    DateTimeOffset CreatedDate { get; set; }
    string CreatedBy { get; set; }
    DateTimeOffset? ModifiedDate { get; set; }
    string? ModifiedBy { get; set; }
    bool IsDeleted { get; set; }
}
