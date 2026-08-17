using BuildFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildFlow.Application.Common.Interfaces;

/// <summary>
/// Abstraction over AppDbContext.
/// Application layer depends on this interface — never on the concrete DbContext.
/// This keeps Application free of any Persistence dependency.
/// </summary>
public interface IAppDbContext
{
    DbSet<Tenant> Tenants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}