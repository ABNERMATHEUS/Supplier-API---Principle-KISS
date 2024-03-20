using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Supplier.Domain.Requests;
using System.Text;

namespace Supplier.API.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MongoDb")!;
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("DatabasePOC");
        services.AddSingleton(mongoClient);
        services.AddSingleton(database);
        services.AddSingleton(x => database.GetCollection<Domain.Entities.Supplier>("Supplier"));
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SupplierCreateRequest>();
        services.AddFluentValidationAutoValidation();
        return services;
    }

    public static IServiceCollection AddAuthencationJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTConfig:PrivateKey").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        return services;
    }
}
