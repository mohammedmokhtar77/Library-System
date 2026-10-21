using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class BookRepository(LibraryContext context) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Books
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Books
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        var bookFromDb = await  GetByIdAsync(book.Id, cancellationToken);
        
        if (bookFromDb is null)
            return false;
        
        bookFromDb.Title = book.Title;
        bookFromDb.ISBN = book.ISBN;
        bookFromDb.AuthorId = book.AuthorId;
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await GetByIdAsync(id, cancellationToken);

        if (book is null)
            return false;

        book.IsDeleted = true;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}