using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Constants;
using LibrarySystem.DAL.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Entities.Models;

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
            var authenticationContext = services.GetRequiredService<AuthenticationContext>();
            await authenticationContext.Database.MigrateAsync();
            
            var dataDirectory = Path.Combine(AppContext.BaseDirectory,
                "Seeding" , 
                "SeedData");

            await AuthorSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Authors.json"));
            await BookSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Books.json"));
            await BorrowerSeeder.SeedAsync(context, Path.Combine(dataDirectory, "Borrowers.json"));
            
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await RoleSeeder.SeedRolesAsync(roleManager);
            await SeedInitialAdminAsync(services);
            
            logger.LogInformation("Seeding done");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured during seeding");
            throw;
        }
    }

    private static async Task SeedInitialAdminAsync(IServiceProvider services)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser { UserName = email, Email = email, FullName = "Administrator" };
            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to create bootstrap administrator: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, RoleNames.Admin))
        {
            var addResult = await userManager.AddToRoleAsync(user, RoleNames.Admin);
            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to assign the Admin role: {errors}");
            }
        }
    }
}
