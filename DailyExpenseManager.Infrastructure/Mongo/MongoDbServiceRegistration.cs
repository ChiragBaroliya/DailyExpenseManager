using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace DailyExpenseManager.Infrastructure.Mongo;

public static class MongoDbServiceRegistration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new MongoDbSettings
        {
            ConnectionString = Environment.GetEnvironmentVariable("MONGODB__CONNECTION__STRING") ?? configuration["MONGODB_CONNECTION_STRING"]!,
            DatabaseName = Environment.GetEnvironmentVariable("MONGODB__DATABASE__NAME") ?? configuration["MONGODB_DATABASE_NAME"]!
        };
        Console.WriteLine("Connection String : " + Environment.GetEnvironmentVariable("MONGODB__CONNECTION__STRING"));
        services.AddSingleton(settings);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));
        services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));
        services.AddScoped<Repositories.IUserRepository, Repositories.UserRepository>();
        services.AddScoped<Repositories.ICategoryRepository, Repositories.CategoryRepository>();
        return services;
    }
}
