using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Seeding;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.API.Extensions;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        
        var logger = services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseInitialization");

        try
        {
            var context = services.GetRequiredService<LibraryContext>();
            await context.Database.MigrateAsync();
            
            var dataDirectory = Path.Combine(
                AppContext.BaseDirectory,
                "Seeding",
                "SeedData");

            logger.LogInformation("Base directory: {BaseDirectory}",
                AppContext.BaseDirectory);

            logger.LogInformation("Seed directory: {SeedDirectory}",
                dataDirectory);

            var authorsPath = Path.Combine(dataDirectory, "Authors.Json");
            var booksPath = Path.Combine(dataDirectory, "Books.Json");
            var borrowersPath = Path.Combine(dataDirectory, "Borrowers.json");

            logger.LogInformation("Authors file exists: {Exists}",
                File.Exists(authorsPath));

            logger.LogInformation("Books file exists: {Exists}",
                File.Exists(booksPath));

            logger.LogInformation("Borrowers file exists: {Exists}",
                File.Exists(borrowersPath));

            logger.LogInformation("Existing authors: {Count}",
                await context.Authors.IgnoreQueryFilters().CountAsync());

            logger.LogInformation("Existing books: {Count}",
                await context.Books.IgnoreQueryFilters().CountAsync());

            logger.LogInformation("Existing borrowers: {Count}",
                await context.Borrowers.IgnoreQueryFilters().CountAsync());

            await AuthorSeeder.SeedAsync(context, authorsPath);
            await BookSeeder.SeedAsync(context, booksPath);
            await BorrowerSeeder.SeedAsync(context, borrowersPath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured during seeding");
            throw;
        }
    }
}