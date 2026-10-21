using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Author author, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}