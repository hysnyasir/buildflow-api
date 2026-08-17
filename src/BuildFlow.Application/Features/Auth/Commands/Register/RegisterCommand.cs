using BuildFlow.Contracts.Auth;
using BuildFlow.SharedKernel.Results;
using MediatR;

namespace BuildFlow.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(
    string CompanyName,
    string Subdomain,
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword
) : IRequest<Result<AuthResponse>>;