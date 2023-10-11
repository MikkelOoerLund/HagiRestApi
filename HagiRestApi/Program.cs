using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceCollection = builder.Services;


        serviceCollection.AddControllers();

        serviceCollection.AddDbContext<UserContext>();
        serviceCollection.AddTransient<UserRepository>();


        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseRouting();

        using var scopeServiceProvider = app.Services.CreateScope();
        var serviceProvider = scopeServiceProvider.ServiceProvider;
        var userContext = serviceProvider.GetRequiredService<UserContext>();
        var database = userContext.Database;
        database.EnsureCreated();


        app.Run();
    }
}