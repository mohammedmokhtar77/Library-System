using LibrarySystem.BLL.DTOs.Borrowers;
using LibrarySystem.BLL.Results;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface IBorrowerService
{
    Task<GenericResult<PaginatedList<BorrowerDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<GenericResult<BorrowerDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<GenericResult<BorrowerDto>> CreateAsync(
        CreateBorrowerDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(
        int id,
        UpdateBorrowerDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}