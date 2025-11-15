using Queue.Mqtt;
using Queue.Mqtt.Handlers;

using Data;
using Data.Repository;
using Data.Services;
using Data.Services.Interfaces;


public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddMqtt(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MqttSettings>(
            builder.Configuration.GetSection("Mqtt"));

        // Add handlers
        builder.Services.AddSingleton<IMqttMessageHandler, ConveyorHandler>();
        builder.Services.AddSingleton<IMqttMessageHandler, BakingHandler>();
        builder.Services.AddSingleton<IMqttMessageHandler, DoughHandler>();
        builder.Services.AddSingleton<IMqttMessageHandler, PackingHandler>();

        // Add router and worker
        builder.Services.AddSingleton<MqttMessageRouter>();
        builder.Services.AddHostedService<MqttListenerService>();

        return builder;
    }

    public static WebApplicationBuilder AddMongo(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoSettings>(
            builder.Configuration.GetSection("Database"));

        builder.Services.AddSingleton<MongoDbContext>();

        // Repositories
        builder.Services.AddSingleton<IConveyorRepository, ConveyorRepository>();
        builder.Services.AddSingleton<IBakingRepository, BakingRepository>();
        builder.Services.AddSingleton<IDoughRepository, DoughRepository>();
        builder.Services.AddSingleton<IPackingRepository, PackingRepository>();

        // Services
        builder.Services.AddSingleton<IConveyorService, ConveyorService>();
        builder.Services.AddSingleton<IBakingService, BakingService>();
        builder.Services.AddSingleton<IDoughService, DoughService>();
        builder.Services.AddSingleton<IPackingService, PackingService>();

        return builder;
    }
}