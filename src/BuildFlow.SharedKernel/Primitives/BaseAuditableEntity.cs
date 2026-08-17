namespace BuildFlow.SharedKernel.Primitives;

/// <summary>
/// Base class for all auditable domain entities.
/// Adds multi-tenancy, full audit trail, and soft-delete support.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public Guid TenantId { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTimeOffset? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    protected BaseAuditableEntity()
    {
    }

    protected BaseAuditableEntity(Guid id) : base(id)
    {
    }
}
