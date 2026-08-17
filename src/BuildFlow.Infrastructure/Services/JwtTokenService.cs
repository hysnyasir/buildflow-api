using BuildFlow.Application.Common.Interfaces;
using BuildFlow.Domain.Interfaces;
using BuildFlow.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BuildFlow.Infrastructure.Services;

/// <summary>
/// Generates JWT access tokens and cryptographically random refresh tokens.
/// </summary>
public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IDateTimeService _dateTimeService;

    public JwtTokenService(
        IConfiguration configuration,
        IDateTimeService dateTimeService)
    {
        _configuration = configuration;
        _dateTimeService = dateTimeService;
    }

    public string GenerateAccessToken(
        Guid userId,
        Guid tenantId,
        string email,
        string fullName,
        string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(AppConstants.Claims.UserId, userId.ToString()),
            new Claim(AppConstants.Claims.TenantId, tenantId.ToString()),
            new Claim(AppConstants.Claims.Email, email),
            new Claim(AppConstants.Claims.FullName, fullName),
            new Claim(AppConstants.Claims.Role, role),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: GetAccessTokenExpiry().UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public DateTimeOffset GetAccessTokenExpiry()
    {
        var minutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
        return _dateTimeService.UtcNow.AddMinutes(minutes);
    }

    public DateTimeOffset GetRefreshTokenExpiry()
    {
        var days = int.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7");
        return _dateTimeService.UtcNow.AddDays(days);
    }
}