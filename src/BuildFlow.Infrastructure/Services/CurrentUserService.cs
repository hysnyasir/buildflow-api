using BuildFlow.Domain.Interfaces;
using BuildFlow.SharedKernel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildFlow.Infrastructure.Services;

/// <summary>
/// Reads the current user's identity from the JWT claims in HttpContext.
/// This is the Infrastructure implementation of the Domain contract ICurrentUserService.
/// Registered as Scoped — one instance per HTTP request.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid UserId =>
        Guid.TryParse(User?.FindFirstValue(AppConstants.Claims.UserId), out var id)
            ? id
            : Guid.Empty;

    public Guid TenantId =>
        Guid.TryParse(User?.FindFirstValue(AppConstants.Claims.TenantId), out var id)
            ? id
            : Guid.Empty;

    public string Email =>
        User?.FindFirstValue(AppConstants.Claims.Email) ?? string.Empty;

    public string FullName =>
        User?.FindFirstValue(AppConstants.Claims.FullName) ?? string.Empty;

    public string Role =>
        User?.FindFirstValue(AppConstants.Claims.Role) ?? string.Empty;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public bool IsSuperAdmin =>
        User?.IsInRole(AppConstants.Roles.SuperAdmin) ?? false;
}
