namespace BuildFlow.Domain.Interfaces;

/// <summary>
/// Provides identity context for the currently authenticated user.
/// Resolved from the JWT token by Infrastructure.CurrentUserService.
/// Available to Application handlers via dependency injection.
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string Email { get; }
    string FullName { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
    bool IsSuperAdmin { get; }
}
