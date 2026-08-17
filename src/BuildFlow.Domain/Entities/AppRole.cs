using Microsoft.AspNetCore.Identity;

namespace BuildFlow.Domain.Entities;

/// <summary>
/// Extends ASP.NET Identity role with BuildFlow-specific fields.
/// </summary>
public sealed class AppRole : IdentityRole<Guid>
{
    public AppRole() { }

    public AppRole(string roleName) : base(roleName) { }
}
