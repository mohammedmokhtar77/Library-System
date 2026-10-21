using System.Text.Json;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Seeding;

public static class BookSeeder
{
    public static async Task SeedAsync(LibraryContext context
        , string filePath, CancellationToken cancellationToken = default)
    {
        if (await context.Books
                .IgnoreQueryFilters()
                .AnyAsync(cancellationToken)
           )
        {
            return;
        }
        
        await using var stream = File.OpenRead(filePath);
        
        var books = await JsonSerializer.DeserializeAsync<List<Book>>(stream, cancellationToken: cancellationToken);
        if (books is null || books.Count == 0)
        {
            return;
        }
        
        context.Books.AddRange(books);
        await context.SaveChangesAsync(cancellationToken);
        
        
    }
}