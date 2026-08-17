using BuildFlow.Application.Common.Interfaces;
using BuildFlow.Domain.Entities;
using BuildFlow.Domain.Interfaces;
using BuildFlow.SharedKernel.Events;
using BuildFlow.SharedKernel.Primitives;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuildFlow.Persistence;

/// <summary>
/// Main EF Core DbContext for BuildFlow.
/// Extends IdentityDbContext to get all ASP.NET Identity tables automatically.
/// Responsibilities:
///   - Auto-sets audit fields on SaveChanges
///   - Applies global query filters (TenantId + IsDeleted)
///   - Dispatches domain events after SaveChanges
/// </summary>
public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IAppDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMediator _mediator;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        IMediator mediator) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
        _mediator = mediator;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync();
        return result;
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker
            .Entries<BaseAuditableEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = _dateTimeService.UtcNow;
                entry.Entity.CreatedBy = _currentUserService.Email;
                entry.Entity.TenantId = _currentUserService.TenantId;
                entry.Entity.IsDeleted = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = _dateTimeService.UtcNow;
                entry.Entity.ModifiedBy = _currentUserService.Email;
            }
        }
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<BaseAuditableEntity>()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvents>()
            .Where(e => e.DomainEvents.Count != 0)
            .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
