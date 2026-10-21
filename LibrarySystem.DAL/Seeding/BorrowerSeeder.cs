using System.Text.Json;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Seeding;

public static class BorrowerSeeder
{
    public static async Task SeedAsync(LibraryContext context,
        string filePath, CancellationToken cancellationToken = default)
    {
        if (await context.Borrowers
                .IgnoreQueryFilters()
                .AnyAsync(cancellationToken))
        {
            return;
        }
        
        var stream = File.OpenRead(filePath);
        var borrowers = await JsonSerializer.DeserializeAsync<List<Borrower>>(stream, cancellationToken: cancellationToken);

        if (borrowers is null || borrowers.Count == 0)
        {
            return;
        }
        
        context.Borrowers.AddRange(borrowers);
        await context.SaveChangesAsync(cancellationToken);
    }
}