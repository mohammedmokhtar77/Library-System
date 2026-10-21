using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Interfaces.Repositories;

public interface IBorrowerRepository
{
    Task<IReadOnlyList<Borrower>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Borrower?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Borrower> AddAsync(Borrower borrower, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Borrower borrower, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}