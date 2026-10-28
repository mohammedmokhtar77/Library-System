using LibrarySystem.BLL.Results;
using LibrarySystem.Entities.Base;

namespace LibrarySystem.BLL.Interfaces.Repositories;

public interface IGenericRepository <T , in TKey> where T 
    : AuditableEntity
{
    Task<PaginatedList<T>>
        GetAllAsync(int pageNumber , int pageSize , CancellationToken cancellationToken);
    
    Task<T?> GetByIdAsync(TKey id , CancellationToken cancellationToken);
    
    Task<T> AddAsync(T entity , CancellationToken cancellationToken);
    
    Task<bool> UpdateAsync(T entity , CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(TKey id , CancellationToken cancellationToken);
}