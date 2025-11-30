using Domains.Blockchain.Application;
using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Sensors.Application;
using Domains.Sensors.Infrastructure;
using Domains.Sensors.Infrastructure.Data;
using Infrastructure.Data.MongoDB;
using MongoDB.Driver;

namespace Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers MongoDB infrastructure and domain services.
    ///     All domains share a single database connection configured at the application level.
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

        // Register shared MongoDB database as singleton
        builder.Services.AddSingleton(mongoDatabase);

        // Register domain settings
        builder.Services.Configure<SensorsSettings>(
            builder.Configuration.GetSection("Sensors"));

        builder.Services.Configure<BlockchainSettings>(
            builder.Configuration.GetSection("Blockchain"));

        // Register Sensors infrastructure and services
        builder.Services.AddScoped<SensorsMongoContext>();
        builder.Services.AddScoped<SensorsUnitOfWork>(sp =>
            new SensorsUnitOfWork(sp.GetRequiredService<SensorsMongoContext>()));
        builder.Services.AddScoped<SensorService>();

        // Register Blockchain infrastructure and services
        builder.Services.AddScoped<WalletMongoContext>();
        builder.Services.AddScoped<WalletUnitOfWork>(sp =>
            new WalletUnitOfWork(sp.GetRequiredService<WalletMongoContext>()));
        builder.Services.AddSingleton<NonceService>();
        builder.Services.AddScoped<WalletService>();

        return builder;
    }
}