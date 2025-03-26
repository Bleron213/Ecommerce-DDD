using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Markers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Extensions;
using Ecommerce.Domain.Entities.Shared.Attributes;

namespace Ecommerce.Infrastructure.Data.Interceptors;
public class AuditTrailInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _user;

    public AuditTrailInterceptor(
        ICurrentUserService user)
    {
        _user = user;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            if (!entry.Entity.GetType().IsAssignableTo(typeof(IAuditable)))
                continue;

            if (entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            var audit = new AuditTrail
            {
                AuditId = Guid.NewGuid(),
                AffectedEntity = entry.Entity.GetType().Name,
                AuditDate = DateTimeOffset.UtcNow,
                CreatedBy = _user.UserGuid.ToString(),
            };

            Dictionary<string, object> oldValues = new();
            Dictionary<string, object> newValues = new();
            foreach (var property in entry.Properties)
            {
                var isAuditTrackeableDefined = Attribute.IsDefined(property.Metadata.PropertyInfo!, typeof(TrackPropertyAttribute));
                var isPk = property.Metadata.IsPrimaryKey();
                if (!isAuditTrackeableDefined && !property.Metadata.IsPrimaryKey())
                    continue;

                var propertyName = property.Metadata.PropertyInfo!.Name;

                if (property.Metadata.IsPrimaryKey())
                    audit.AffectedEntityId = (Guid)property.CurrentValue!;

                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            audit.AuditType = AuditType.Added;
                            newValues[propertyName.ToSnakeCase()] = property.CurrentValue!;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            audit.AuditType = AuditType.Updated;
                            oldValues[propertyName.ToSnakeCase()] = property.CurrentValue!;
                            newValues[propertyName.ToSnakeCase()] = property.CurrentValue!;
                            break;
                        }
                }

                audit.NewValues = newValues;

                audit.OldValues = oldValues;
            }
            context.Add(audit);
        }

    }
}

