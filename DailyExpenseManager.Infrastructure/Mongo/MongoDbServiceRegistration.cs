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
            ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? configuration["MONGODB_CONNECTION_STRING"]!,
            DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? configuration["MONGODB_DATABASE_NAME"]!
        };
        Console.WriteLine("Connection String : " + Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING"));
        services.AddSingleton(settings);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));
        services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));
        services.AddScoped<Repositories.IUserRepository, Repositories.UserRepository>();
        return services;
    }
}
