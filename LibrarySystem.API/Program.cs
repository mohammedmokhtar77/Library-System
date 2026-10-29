using LibrarySystem.API.Extensions;
using LibrarySystem.API.Middlewares;

namespace LibrarySystem.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();

        builder.Services.AddSwaggerServices();
        builder.Services.AddDatabaseServices(builder.Configuration);
        builder.Services.AddRepositoryServices();
        builder.Services.AddMappingServices();

        var app = builder.Build();

        await app.SeedDatabaseAsync();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<RequestIdHeaderMiddleware>();
        app.UseMiddleware<RequestTimingMiddleware>();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        await app.RunAsync();
    }
}