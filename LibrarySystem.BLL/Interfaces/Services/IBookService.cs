using LibrarySystem.BLL.DTOs.Books;
using LibrarySystem.BLL.Results;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface IBookService
{
    Task<GenericResult<PaginatedList<BookDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<GenericResult<BookDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<GenericResult<BookDto>> CreateAsync(
        CreateBookDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}