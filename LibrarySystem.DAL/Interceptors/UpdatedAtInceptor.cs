using LibrarySystem.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibrarySystem.DAL.Interceptors;

public class UpdatedAtInceptor  : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyUpdatedAt(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyUpdatedAt(eventData);
        return ValueTask.FromResult(result);
    }

    private void ApplyUpdatedAt(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State != EntityState.Modified)
                continue;
            
            entry.Entity.UpdatedAt =  DateTime.UtcNow;
        }
    }
}