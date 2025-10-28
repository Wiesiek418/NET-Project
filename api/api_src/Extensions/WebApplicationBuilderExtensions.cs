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
        builder.Services.AddSingleton<IMqttMessageHandler, AlphaHandler>();

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

        builder.Services.AddSingleton<IAlphaRepository, AlphaRepository>();

        builder.Services.AddSingleton<IAlphaService, AlphaService>();

        return builder;
    }
}