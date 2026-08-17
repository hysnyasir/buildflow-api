using BuildFlow.SharedKernel.Primitives;

namespace BuildFlow.Domain.Entities;

/// <summary>
/// Represents a company (tenant) that subscribes to BuildFlow.
/// This is the root of multi-tenancy — every other entity belongs to a Tenant.
/// Inherits BaseEntity only (not BaseAuditableEntity) because Tenant
/// IS the tenant — it cannot reference itself via TenantId.
/// </summary>
public sealed class Tenant : BaseEntity
{
    public string Name { get; init; } = null!;
    public string Subdomain { get; init; } = null!;
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedDate { get; init; }

    private Tenant() { }  // EF Core

    public static Tenant Create(string name, string subdomain)
    {
        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            Subdomain = subdomain.ToLowerInvariant().Trim(),
            IsActive = true,
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}
