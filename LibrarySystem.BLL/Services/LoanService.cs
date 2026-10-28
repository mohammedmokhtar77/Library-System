using AutoMapper;
using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Services;

public sealed class LoanService(
    ILoanRepository loanRepository,
    IBookRepository bookRepository,
    IBorrowerRepository borrowerRepository,
    IMapper mapper) : ILoanService
{
    public async Task<GenericResult<IReadOnlyList<LoanDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var loans = await loanRepository.GetAllAsync(
            cancellationToken);

        var loanDtos = mapper.Map<IReadOnlyList<LoanDto>>(loans);

        return GenericResult<IReadOnlyList<LoanDto>>.Success(
            loanDtos);
    }

    public async Task<GenericResult<LoanDto>> GetByIdAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default)
    {
        if (bookId <= 0 || borrowerId <= 0)
            return GenericResult<LoanDto>.Failure(
                "Book ID and Borrower ID must be greater than zero.");

        var loan = await loanRepository.GetByIdAsync(
            bookId,
            borrowerId,
            cancellationToken);

        if (loan is null)
            return GenericResult<LoanDto>.Failure(
                "Loan was not found.");

        return GenericResult<LoanDto>.Success(
            mapper.Map<LoanDto>(loan));
    }

    public async Task<GenericResult<LoanDto>> CreateAsync(
        CreateLoanDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.BookId <= 0 || dto.BorrowerId <= 0)
            return GenericResult<LoanDto>.Failure(
                "Book ID and Borrower ID must be greater than zero.");

        if (dto.LoanDate == default)
            return GenericResult<LoanDto>.Failure(
                "Loan date is required.");

        if (dto.LoanDate.Date > DateTime.UtcNow.Date)
            return GenericResult<LoanDto>.Failure(
                "Loan date cannot be in the future.");

        if (dto.ReturnDate.HasValue &&
            dto.ReturnDate.Value < dto.LoanDate)
        {
            return GenericResult<LoanDto>.Failure(
                "Return date cannot be earlier than loan date.");
        }

        var book = await bookRepository.GetByIdAsync(
            dto.BookId,
            cancellationToken);

        if (book is null)
            return GenericResult<LoanDto>.Failure(
                "Book was not found.");

        var borrower = await borrowerRepository.GetByIdAsync(
            dto.BorrowerId,
            cancellationToken);

        if (borrower is null)
            return GenericResult<LoanDto>.Failure(
                "Borrower was not found.");

        var existingLoan = await loanRepository.GetByIdAsync(
            dto.BookId,
            dto.BorrowerId,
            cancellationToken);

        if (existingLoan is not null)
            return GenericResult<LoanDto>.Failure(
                "A loan record already exists for this book and borrower.");

        var loan = mapper.Map<Loan>(dto);

        var createdLoan = await loanRepository.AddAsync(
            loan,
            cancellationToken);

        // Load navigation properties for the response DTO.
        var savedLoan = await loanRepository.GetByIdAsync(
            createdLoan.BookId,
            createdLoan.BorrowerId,
            cancellationToken);

        return GenericResult<LoanDto>.Success(
            mapper.Map<LoanDto>(savedLoan ?? createdLoan));
    }

    public async Task<Result> UpdateAsync(
        int bookId,
        int borrowerId,
        UpdateLoanDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (bookId <= 0 || borrowerId <= 0)
            return Result.Failure(
                "Book ID and Borrower ID must be greater than zero.");

        if (dto.LoanDate == default)
            return Result.Failure("Loan date is required.");

        if (dto.LoanDate.Date > DateTime.UtcNow.Date)
            return Result.Failure(
                "Loan date cannot be in the future.");

        if (dto.ReturnDate.HasValue &&
            dto.ReturnDate.Value < dto.LoanDate)
        {
            return Result.Failure(
                "Return date cannot be earlier than loan date.");
        }

        var loan = await loanRepository.GetByIdAsync(
            bookId,
            borrowerId,
            cancellationToken);

        if (loan is null)
            return Result.Failure("Loan was not found.");

        mapper.Map(dto, loan);

        var updated = await loanRepository.UpdateAsync(
            loan,
            cancellationToken);

        return updated
            ? Result.Success()
            : Result.Failure("Unable to update the loan.");
    }

    public async Task<Result> DeleteAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default)
    {
        if (bookId <= 0 || borrowerId <= 0)
            return Result.Failure(
                "Book ID and Borrower ID must be greater than zero.");

        var loan = await loanRepository.GetByIdAsync(
            bookId,
            borrowerId,
            cancellationToken);

        if (loan is null)
            return Result.Failure("Loan was not found.");

        var deleted = await loanRepository.DeleteAsync(
            bookId,
            borrowerId,
            cancellationToken);

        return deleted
            ? Result.Success()
            : Result.Failure("Unable to delete the loan.");
    }
}
