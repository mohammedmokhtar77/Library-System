using LibrarySystem.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibrarySystem.DAL.Interceptors;

public class CreatedAtInceptor  : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyCreatedAt(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyCreatedAt(eventData);
        return ValueTask.FromResult(result);
    }

    private void ApplyCreatedAt(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State != EntityState.Added)
                continue;
            
            entry.Entity.CreatedAt = DateTime.UtcNow;
        }
    }
}