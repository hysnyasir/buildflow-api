namespace BuildFlow.Application.Common.Interfaces;

/// <summary>
/// Generates JWT access tokens and refresh tokens.
/// Defined in Application — implemented in Infrastructure.
/// </summary>
public interface IJwtTokenService
{
    string GenerateAccessToken(
        Guid userId,
        Guid tenantId,
        string email,
        string fullName,
        string role);

    string GenerateRefreshToken();

    DateTimeOffset GetAccessTokenExpiry();
    DateTimeOffset GetRefreshTokenExpiry();
}