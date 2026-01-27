using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace DailyExpenseManager.Infrastructure.Mongo;

public static class MongoDbServiceRegistration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
       Environment.GetEnvironmentVariable("MONGODB__CONNECTION__STRING")
       ?? configuration["MONGODB_CONNECTION_STRING"];

        var databaseName =
            Environment.GetEnvironmentVariable("MONGODB__DATABASE__NAME")
            ?? configuration["MONGODB_DATABASE_NAME"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("MongoDB connection string is missing");

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new Exception("MongoDB database name is missing");

        Console.WriteLine($"MongoDB Database: {databaseName}");

        var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
        mongoSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(5);

        var client = new MongoClient(mongoSettings);

        services.AddSingleton(client);
        services.AddScoped(sp => client.GetDatabase(databaseName));

        services.AddScoped<Repositories.IUserRepository, Repositories.UserRepository>();
        services.AddScoped<Repositories.ICategoryRepository, Repositories.CategoryRepository>();

        return services;
    }
}
