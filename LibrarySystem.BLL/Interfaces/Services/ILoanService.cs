using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.BLL.Results;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface ILoanService
{
    Task<GenericResult<IReadOnlyList<LoanDto>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<GenericResult<LoanDto>> GetByIdAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default);

    Task<GenericResult<LoanDto>> CreateAsync(
        CreateLoanDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(
        int bookId,
        int borrowerId,
        UpdateLoanDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default);
}