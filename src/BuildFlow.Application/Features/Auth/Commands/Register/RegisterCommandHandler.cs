using BuildFlow.Application.Common.Interfaces;
using BuildFlow.Contracts.Auth;
using BuildFlow.Domain.Entities;
using BuildFlow.SharedKernel;
using BuildFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BuildFlow.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    private readonly IAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        IAppDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var subdomainTaken = _context.Tenants
            .Any(t => t.Subdomain == request.Subdomain.ToLowerInvariant().Trim());

        if (subdomainTaken)
        {
            return Result.Failure<AuthResponse>(
                Error.Conflict("Tenant.SubdomainTaken",
                    $"Subdomain '{request.Subdomain}' is already taken."));
        }

        var tenant = Tenant.Create(request.CompanyName, request.Subdomain);
        await _context.Tenants.AddAsync(tenant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            TenantId = tenant.Id,
            IsActive = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray());

            throw new SharedKernel.Exceptions.ValidationException(errors);
        }

        if (!await _roleManager.RoleExistsAsync(AppConstants.Roles.TenantAdmin))
        {
            await _roleManager.CreateAsync(
                new AppRole(AppConstants.Roles.TenantAdmin));
        }

        await _userManager.AddToRoleAsync(user, AppConstants.Roles.TenantAdmin);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiry();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshTokenExpiry;
        await _userManager.UpdateAsync(user);

        var accessToken = _jwtTokenService.GenerateAccessToken(
            user.Id,
            tenant.Id,
            user.Email!,
            user.FullName,
            AppConstants.Roles.TenantAdmin);

        return Result.Success(new AuthResponse(
            UserId: user.Id,
            TenantId: tenant.Id,
            FullName: user.FullName,
            Email: user.Email!,
            Role: AppConstants.Roles.TenantAdmin,
            AccessToken: accessToken,
            AccessTokenExpiry: _jwtTokenService.GetAccessTokenExpiry(),
            RefreshToken: refreshToken,
            RefreshTokenExpiry: refreshTokenExpiry));
    }
}