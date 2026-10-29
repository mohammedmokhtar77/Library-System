using LibrarySystem.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibrarySystem.DAL.Interceptors;

public class SoftDeleteInceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplySoftDelete(eventData);
        return ValueTask.FromResult(result);
    }

    private void ApplySoftDelete(DbContextEventData eventData)
    {
        if(eventData.Context is null)
            return;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            
            entry.Property(e => e.CreatedBy).IsModified = false;
        }
    }
}