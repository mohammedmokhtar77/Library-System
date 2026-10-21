using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class AuthorRepository(LibraryContext context) : IAuthorRepository
{
    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Authors
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Authors
            .FirstOrDefaultAsync(a => a.Id == id , cancellationToken);
    }

    public async Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        context.Authors.Add(author);
        await context.SaveChangesAsync(cancellationToken);
        return author;
    }

    public async Task<bool> UpdateAsync(Author author, CancellationToken cancellationToken = default)
    {
        var authorFromDb = await GetByIdAsync(author.Id, cancellationToken);
        if (authorFromDb is null)
            return false;
        authorFromDb.Name = author.Name;
        authorFromDb.BirthDate = author.BirthDate;
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await GetByIdAsync(id, cancellationToken);
        if(author is null)
            return false;

        author.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}