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
        
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.AllowTrailingCommas = true;
        });

        builder.Services.AddSwaggerServices();
        builder.Services.AddDatabaseServices(builder.Configuration);
        builder.Services.AddAuthenticationServices(builder.Configuration);
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
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        await app.RunAsync();
    }
}
