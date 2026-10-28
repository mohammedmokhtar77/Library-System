using AutoMapper;
using LibrarySystem.BLL.DTOs.Books;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Services;

public sealed class BookService(
    IBookRepository repository,
    IMapper mapper) : IBookService
{
    public async Task<GenericResult<PaginatedList<BookDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            return GenericResult<PaginatedList<BookDto>>.Failure(
                "Page number must be greater than zero.");

        if (pageSize is < 1 or > 100)
            return GenericResult<PaginatedList<BookDto>>.Failure(
                "Page size must be between 1 and 100.");

        var books = await repository.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        var bookDtos = mapper.Map<IReadOnlyList<BookDto>>(
            books.Items);

        var result = new PaginatedList<BookDto>(
            bookDtos,
            books.TotalCount,
            books.PageNumber,
            books.PageSize);

        return GenericResult<PaginatedList<BookDto>>.Success(result);
    }

    public async Task<GenericResult<BookDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return GenericResult<BookDto>.Failure(
                "Book ID must be greater than zero.");

        var book = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (book is null)
            return GenericResult<BookDto>.Failure(
                "Book was not found.");

        return GenericResult<BookDto>.Success(
            mapper.Map<BookDto>(book));
    }

    public async Task<GenericResult<BookDto>> CreateAsync(
        CreateBookDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.Title))
            return GenericResult<BookDto>.Failure(
                "Book title is required.");

        if (dto.AuthorId <= 0)
            return GenericResult<BookDto>.Failure(
                "A valid Author ID is required.");

        // Verify that the referenced author exists.
        var author = await repository.GetByIdAsync(
            dto.AuthorId,
            cancellationToken);

        if (author is null)
            return GenericResult<BookDto>.Failure(
                "Author was not found.");

        var book = mapper.Map<Book>(dto);

        var createdBook = await repository.AddAsync(
            book,
            cancellationToken);

        return GenericResult<BookDto>.Success(
            mapper.Map<BookDto>(createdBook));
    }

    public async Task<Result> UpdateAsync(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (id <= 0)
            return Result.Failure("Book ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            return Result.Failure("Book title is required.");

        if (dto.AuthorId <= 0)
            return Result.Failure("A valid Author ID is required.");

        var book = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (book is null)
            return Result.Failure("Book was not found.");

        var author = await repository.GetByIdAsync(
            dto.AuthorId,
            cancellationToken);

        if (author is null)
            return Result.Failure("Author was not found.");

        mapper.Map(dto, book);

        var updated = await repository.UpdateAsync(
            book,
            cancellationToken);

        return updated
            ? Result.Success()
            : Result.Failure("Unable to update the book.");
    }

    public async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure("Book ID must be greater than zero.");

        var book = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (book is null)
            return Result.Failure("Book was not found.");

        var deleted = await repository.DeleteAsync(
            id,
            cancellationToken);

        return deleted
            ? Result.Success()
            : Result.Failure("Unable to delete the book.");
    }
}
