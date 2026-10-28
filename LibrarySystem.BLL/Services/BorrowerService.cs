using AutoMapper;
using LibrarySystem.BLL.DTOs.Borrowers;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Services;

public sealed class BorrowerService(
    IBorrowerRepository repository,
    IMapper mapper) : IBorrowerService
{
    public async Task<GenericResult<PaginatedList<BorrowerDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            return GenericResult<PaginatedList<BorrowerDto>>.Failure(
                "Page number must be greater than zero.");

        if (pageSize is < 1 or > 100)
            return GenericResult<PaginatedList<BorrowerDto>>.Failure(
                "Page size must be between 1 and 100.");

        var borrowers = await repository.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        var borrowerDtos = mapper.Map<IReadOnlyList<BorrowerDto>>(
            borrowers.Items);

        var result = new PaginatedList<BorrowerDto>(
            borrowerDtos,
            borrowers.TotalCount,
            borrowers.PageNumber,
            borrowers.PageSize);

        return GenericResult<PaginatedList<BorrowerDto>>.Success(result);
    }

    public async Task<GenericResult<BorrowerDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return GenericResult<BorrowerDto>.Failure(
                "Borrower ID must be greater than zero.");

        var borrower = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (borrower is null)
            return GenericResult<BorrowerDto>.Failure(
                "Borrower was not found.");

        return GenericResult<BorrowerDto>.Success(
            mapper.Map<BorrowerDto>(borrower));
    }

    public async Task<GenericResult<BorrowerDto>> CreateAsync(
        CreateBorrowerDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.Name))
            return GenericResult<BorrowerDto>.Failure(
                "Borrower name is required.");

        if (dto.Name.Length > 100)
            return GenericResult<BorrowerDto>.Failure(
                "Borrower name cannot exceed 100 characters.");

        if (dto.MembershipDate.Date > DateTime.UtcNow.Date)
            return GenericResult<BorrowerDto>.Failure(
                "Membership date cannot be in the future.");

        var borrower = mapper.Map<Borrower>(dto);

        var createdBorrower = await repository.AddAsync(
            borrower,
            cancellationToken);

        return GenericResult<BorrowerDto>.Success(
            mapper.Map<BorrowerDto>(createdBorrower));
    }

    public async Task<Result> UpdateAsync(
        int id,
        UpdateBorrowerDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (id <= 0)
            return Result.Failure(
                "Borrower ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result.Failure(
                "Borrower name is required.");

        if (dto.Name.Length > 100)
            return Result.Failure(
                "Borrower name cannot exceed 100 characters.");

        if (dto.MembershipDate.Date > DateTime.UtcNow.Date)
            return Result.Failure(
                "Membership date cannot be in the future.");

        var borrower = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (borrower is null)
            return Result.Failure("Borrower was not found.");

        mapper.Map(dto, borrower);

        var updated = await repository.UpdateAsync(
            borrower,
            cancellationToken);

        return updated
            ? Result.Success()
            : Result.Failure("Unable to update the borrower.");
    }

    public async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure(
                "Borrower ID must be greater than zero.");

        var borrower = await repository.GetByIdAsync(
            id,
            cancellationToken);

        if (borrower is null)
            return Result.Failure("Borrower was not found.");

        var deleted = await repository.DeleteAsync(
            id,
            cancellationToken);

        return deleted
            ? Result.Success()
            : Result.Failure("Unable to delete the borrower.");
    }
}
