using HagiDatabaseDomain;
using HagiRestApi.Controllers;
using HagiRestApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;

class Program
{


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceCollection = builder.Services;
        var configurations = builder.Configuration;

        var jsonWebTokenConfiguration = new JsonWebTokenConfiguration();
        configurations.Bind("JsonWebTokenConfiguration", jsonWebTokenConfiguration);


        serviceCollection
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                var serializerSettings = options.SerializerSettings;
                serializerSettings.ContractResolver = new DefaultContractResolver();
                serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        serviceCollection.AddDbContext<UserContext>();
        serviceCollection.AddTransient<UserRepository>();


        var authenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        var securityKey = jsonWebTokenConfiguration.Key;
        var secutiryKeyBytes = Encoding.ASCII.GetBytes(securityKey);


        serviceCollection.AddAuthentication(authenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jsonWebTokenConfiguration.Issuer,
                    ValidAudience = jsonWebTokenConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secutiryKeyBytes)
                };
            });


        serviceCollection.AddSingleton<UserConverter>()
            .AddSingleton<JsonWebTokenConfiguration>(jsonWebTokenConfiguration);
        
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