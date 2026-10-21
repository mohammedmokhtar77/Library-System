using System.Text.Json;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Seeding;

public static class AuthorSeeder
{
    public static async Task SeedAsync(LibraryContext context,
        string filePath, CancellationToken cancellationToken = default
    )
    {
        if (await context.Authors
                .IgnoreQueryFilters()
                .AnyAsync(cancellationToken))
        {
            return;
        }

        await using var stream = File.OpenRead(filePath);
        
        var authors = await JsonSerializer.DeserializeAsync
            <List<Author>>(stream, cancellationToken: cancellationToken);

        if (authors is null || authors.Count == 0)
        {
            return;
        }
        
        context.Authors.AddRange(authors);
        await context.SaveChangesAsync(cancellationToken);
    }
}