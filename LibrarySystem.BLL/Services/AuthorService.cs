using AutoMapper;
using LibrarySystem.BLL.DTOs.Authors;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Services;

public class AuthorService(IAuthorRepository repository, IMapper mapper)
    : IAuthorService
{
    public async Task<GenericResult<PaginatedList<AuthorDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            return GenericResult<PaginatedList<AuthorDto>>
                .Failure("Page number must be greater than zero.");

        if (pageSize is < 1 or > 100)
            return GenericResult<PaginatedList<AuthorDto>>
                .Failure("Page size must be between 1 and 100.");

        var authors = await repository.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        var authorDtos = mapper.Map<IReadOnlyList<AuthorDto>>(authors.Items);

        var paginatedDtos = new PaginatedList<AuthorDto>(
            authorDtos,
            authors.TotalCount,
            authors.PageNumber,
            authors.PageSize);

        return GenericResult<PaginatedList<AuthorDto>>.Success(paginatedDtos);
    }

    public async Task<GenericResult<AuthorDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return GenericResult<AuthorDto>.Failure(
                "Author ID must be greater than zero.");

        var author = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (author is null)
            return GenericResult<AuthorDto>.Failure(
                "Author was not found.");

        var authorDto = mapper.Map<AuthorDto>(author);

        return GenericResult<AuthorDto>.Success(authorDto);
    }
    
    public async Task<GenericResult<AuthorDto>> CreateAsync(
        CreateAuthorDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.Name))
            return GenericResult<AuthorDto>.Failure(
                "Author name is required.");

        if (dto.BirthDate > DateTime.UtcNow)
            return GenericResult<AuthorDto>.Failure(
                "Birth date cannot be in the future.");

        var author = mapper.Map<Author>(dto);

        var createdAuthor = await repository.AddAsync(
            author,
            cancellationToken);

        var authorDto = mapper.Map<AuthorDto>(createdAuthor);

        return GenericResult<AuthorDto>.Success(authorDto);
    }

    public async Task<Result> UpdateAsync(
        int id,
        UpdateAuthorDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (id <= 0)
            return Result.Failure("Author ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result.Failure("Author name is required.");

        if (dto.BirthDate > DateTime.UtcNow)
            return Result.Failure("Birth date cannot be in the future.");

        var author = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (author is null)
            return Result.Failure("Author was not found.");

        mapper.Map(dto, author);

        var updated = await repository.UpdateAsync(
            author,
            cancellationToken);

        if (!updated)
            return Result.Failure("Unable to update the author.");

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure("Author ID must be greater than zero.");

        var author = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (author is null)
            return Result.Failure("Author was not found.");

        var deleted = await repository.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
            return Result.Failure("Unable to delete the author.");

        return Result.Success();
    }
}
