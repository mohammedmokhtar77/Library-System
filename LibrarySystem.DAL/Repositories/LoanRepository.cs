using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class LoanRepository(LibraryContext context) : ILoanRepository
{
    public async Task<IReadOnlyList<Loan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Loans
            .AsNoTracking()
            .Include(b => b.Book)
            .Include(b => b.Borrower)
            .ToListAsync(cancellationToken);
    }

    public Task<Loan?> GetByIdAsync(int bookId, int borrowerId, CancellationToken cancellationToken = default)
    {
        return context.Loans
            .Include(b => b.Book)
            .Include(b => b.Borrower)
            .FirstOrDefaultAsync(
                x =>
                    x.BookId == bookId && x.BorrowerId == borrowerId
                , cancellationToken);
    }

    public async Task<Loan> AddAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        context.Loans.Add(loan);
        await context.SaveChangesAsync(cancellationToken);
        return loan;
    }

    public async Task<bool> UpdateAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        var loanFromDb = await GetByIdAsync(loan.BookId, loan.BorrowerId, cancellationToken);
        
        if(loanFromDb is null)
            return false;
        loanFromDb.LoanDate = loan.LoanDate;
        loanFromDb.ReturnDate = loan.ReturnDate;   
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int bookId, int borrowerId, CancellationToken cancellationToken = default)
    {
        var loan = await GetByIdAsync(bookId, borrowerId, cancellationToken);
        
        if (loan is null) 
            return false;

        loan.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}