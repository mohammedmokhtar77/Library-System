using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class BorrowerRepository(LibraryContext context) : IBorrowerRepository
{
    public async Task<IReadOnlyList<Borrower>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Borrowers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Borrower?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Borrowers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Borrower> AddAsync(Borrower borrower, CancellationToken cancellationToken = default)
    {
        context.Borrowers.Add(borrower);
        await context.SaveChangesAsync(cancellationToken);
        return borrower;
    }

    public async Task<bool> UpdateAsync(Borrower borrower, CancellationToken cancellationToken = default)
    {
        var borrowerFromDb = await GetByIdAsync(borrower.Id, cancellationToken);

        if (borrowerFromDb is null)
            return false;

        borrowerFromDb.Name = borrower.Name;
        borrowerFromDb.MembershipDate = borrower.MembershipDate;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var borrower = await GetByIdAsync(id, cancellationToken);

        if (borrower is null)
            return false;

        borrower.IsDeleted = true;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}