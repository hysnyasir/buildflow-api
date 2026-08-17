namespace BuildFlow.Contracts.Auth;

public sealed record AuthResponse(
    Guid UserId,
    Guid TenantId,
    string FullName,
    string Email,
    string Role,
    string AccessToken,
    DateTimeOffset AccessTokenExpiry,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiry
);