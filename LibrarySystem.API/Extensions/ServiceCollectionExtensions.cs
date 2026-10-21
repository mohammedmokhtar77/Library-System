using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOpenApi();
        return services;
    }
   
    public static IServiceCollection AddDatabaseServices
       (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
        return services;
    }

    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBorrowerRepository, BorrowerRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        return services;
    }
}