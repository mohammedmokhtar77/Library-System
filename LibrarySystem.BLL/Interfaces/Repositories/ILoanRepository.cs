using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Interfaces.Repositories;

public interface ILoanRepository
{
    Task<IReadOnlyList<Loan>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Loan?> GetByIdAsync(int bookId, int borrowerId, CancellationToken cancellationToken = default);

    Task<Loan> AddAsync(Loan loan, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Loan loan, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int bookId, int borrowerId, CancellationToken cancellationToken = default);
}