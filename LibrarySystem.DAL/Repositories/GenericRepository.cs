using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Results;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class GenericRepository<T , TKey>(LibraryContext context) : 
    IGenericRepository<T , TKey> where T : AuditableEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        
        var query = _dbSet.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        var paginatedList = new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
        
        return paginatedList;
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync(id, cancellationToken);
        if (entity is null || entity.IsDeleted)
            return null;
        return entity;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await  _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        if (entity.IsDeleted)
            return false;
        
        var entry = context.Entry(entity);

        if (entry.State == EntityState.Detached)
            _dbSet.Update(entity);
        
        var affectedRows = await context.SaveChangesAsync(cancellationToken);
        
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        
        if(entity is null) 
            return false;
        entity.IsDeleted = true; // Soft Delete

        var affectedRows = await context.SaveChangesAsync(cancellationToken);
        
        return affectedRows > 0;
    }
}