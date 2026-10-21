using LibrarySystem.API.Extensions;
using LibrarySystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();

        builder.Services.AddSwaggerServices();
        builder.Services.AddDatabaseServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}