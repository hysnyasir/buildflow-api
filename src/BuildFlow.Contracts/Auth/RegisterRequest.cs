namespace BuildFlow.Contracts.Auth;

public sealed record RegisterRequest(
    string CompanyName,
    string Subdomain,
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword
);
