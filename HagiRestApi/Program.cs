using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

class Program
{


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceCollection = builder.Services;


        serviceCollection
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

        serviceCollection.AddDbContext<UserContext>();
        serviceCollection.AddTransient<UserRepository>();
        serviceCollection.AddSingleton<UserConverter>();

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