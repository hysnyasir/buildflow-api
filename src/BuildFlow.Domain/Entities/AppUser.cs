using Microsoft.AspNetCore.Identity;

namespace BuildFlow.Domain.Entities;

/// <summary>
/// Extends ASP.NET Identity user with BuildFlow-specific fields.
/// Uses Guid as the primary key to be consistent with all other entities.
/// Does NOT inherit BaseAuditableEntity — IdentityUser is the base class.
/// </summary>
public sealed class AppUser : IdentityUser<Guid>
{
    public string FullName { get; init; } = null!;
    public Guid TenantId { get; init; }
    public bool IsActive { get; init; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiry { get; set; }
}
