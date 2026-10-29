using LibrarySystem.BLL.DTOs.Mapping;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Services;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Interceptors;
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
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(new SoftDeleteInceptor() , new CreatedAtInceptor() , new UpdatedAtInceptor())
        );
        return services;
    }

    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBorrowerRepository, BorrowerRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBorrowerService, BorrowerService>();
        services.AddScoped<ILoanService, LoanService>();
        return services;
    }

    public static IServiceCollection AddMappingServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AuthorProfile>();
            cfg.AddProfile<BookProfile>();
            cfg.AddProfile<BorrowerProfile>();
            cfg.AddProfile<LoanProfile>();
        });
        return services;
    }
}