using System.Text;
using LibrarySystem.BLL.DTOs.Mapping;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Services;
using LibrarySystem.DAL.Data;
using LibrarySystem.DAL.Interceptors;
using LibrarySystem.DAL.Repositories;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.OpenApi;

namespace LibrarySystem.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token. Swagger will add the Bearer prefix."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });   
        
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

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuthenticationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AuthConnection")));
        
        services.AddIdentity<ApplicationUser , IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
        });
        
        services.AddScoped<IJwtService, JwtService>();
        
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
        services.AddScoped<IUserService, UserService>();
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