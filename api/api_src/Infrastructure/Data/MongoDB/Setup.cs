using MongoDB.Driver;
using Infrastructure.Data.MongoDB;

namespace Infrastructure.Data.MongoDB;

public static class MongoInfrastructureSetup
{
    /// <summary>
    /// Registers MongoDB infrastructure and all domain services.
    /// Sets up shared database connection and delegates to domain-specific registration methods.
    /// </summary>
    public static WebApplicationBuilder AddMongoInfrastructure(this WebApplicationBuilder builder)
    {
        // Register shared database settings from configuration
        builder.Services.Configure<MongoDBSettings>(
            builder.Configuration.GetSection("Database"));

        var databaseSettings = builder.Configuration
            .GetSection("Database")
            .Get<MongoDBSettings>() ?? new MongoDBSettings();

        // Create single shared MongoDB client and database
        var mongoClient = new MongoClient(databaseSettings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.DatabaseName);

        // Register the MongoDB database instance for dependency injection
        builder.Services.AddSingleton(mongoDatabase);

        // Register health check
        builder.Services
            .AddSingleton(mongoClient)
            .AddHealthChecks()
            .AddMongoDb();

        return builder;
    }
}