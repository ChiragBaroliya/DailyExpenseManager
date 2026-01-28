using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;

namespace DailyExpenseManager.Infrastructure.Mongo;

public static class MongoDbServiceRegistration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        try
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
            Console.WriteLine($"MongoDB ConnectionString: {connectionString}");

            var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
            mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
            mongoSettings.UseTls = true;

            var client = new MongoClient(mongoSettings);

            services.AddSingleton(client);
            services.AddScoped(sp => client.GetDatabase(databaseName));

            services.AddScoped<Repositories.IUserRepository, Repositories.UserRepository>();
            services.AddScoped<Repositories.ICategoryRepository, Repositories.CategoryRepository>();

           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return services;
    }
}
