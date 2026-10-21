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
            
            var dataDirectory = Path.Combine(AppContext.BaseDirectory,
                "Seeding" , 
                "SeedData");

            await AuthorSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Authors.Json"));
            await BookSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Books.Json"));
            await BorrowerSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Borrowers.json"));
            logger.LogInformation("Seeding done");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured during seeding");
            throw;
        }
    }
}