using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController(ILoanRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LoanDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var loans = await repository.GetAllAsync(cancellationToken);

        var loanDto = loans.Select(ToDto).ToList();

        return Ok(loanDto);
    }

    [HttpGet("{bookId:int}/{borrowerId:int}")]
    public async Task<ActionResult<LoanDto>> GetByIdAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default)
    {
        var loan = await repository.GetByIdAsync(
            bookId, borrowerId, cancellationToken);

        if (loan is null)
            return NotFound();

        return Ok(ToDto(loan));
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> CreateAsync(
        CreateLoanDto loanDto,
        CancellationToken cancellationToken = default)
    {
        var loan = new Loan
        {
            BookId = loanDto.BookId,
            BorrowerId = loanDto.BorrowerId,
            LoanDate = loanDto.LoanDate,
            ReturnDate = loanDto.ReturnDate
        };

        var createdLoan = await repository.AddAsync(
            loan, cancellationToken);

        var result = await repository.GetByIdAsync(
            createdLoan.BookId,
            createdLoan.BorrowerId,
            cancellationToken);

        if (result is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new
            {
                bookId = createdLoan.BookId,
                borrowerId = createdLoan.BorrowerId
            },
            ToDto(result));
    }

    [HttpPut("{bookId:int}/{borrowerId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int bookId,
        int borrowerId,
        UpdateLoanDto loanDto,
        CancellationToken cancellationToken = default)
    {
        var updated = await repository.UpdateAsync(new Loan()
        {
            BookId = bookId,
            BorrowerId = borrowerId,
            LoanDate = loanDto.LoanDate,
            ReturnDate = loanDto.ReturnDate,
        }, cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{bookId:int}/{borrowerId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(
            bookId, borrowerId, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    private static LoanDto ToDto(Loan loan) => new()
    {
        BookId = loan.BookId,
        BookTitle = loan.Book.Title,
        BorrowerId = loan.BorrowerId,
        BorrowerName = loan.Borrower.Name,
        LoanDate = loan.LoanDate,
        ReturnDate = loan.ReturnDate
    };
}