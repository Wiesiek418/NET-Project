using Microsoft.Extensions.Options;
using Infrastructure.Mqtt;
using Domains.Sensors.EventHandlers;
using Domains.Sensors.Infrastructure;

namespace Extensions;

/// <summary>
/// Extension methods for configuring MQTT infrastructure with the new clean architecture.
/// Registers message router, listener service, handlers, and work queue.
/// Handlers are registered as Transient to allow proper scoped dependency injection.
/// Settings are registered using the Options pattern with configuration binding.
/// </summary>
public static class MqttInfrastructureExtensions
{
    /// <summary>
    /// Adds MQTT infrastructure services and handlers.
    /// </summary>
    /// <remarks>
    /// This method should be called after AddMongoInfrastructure to ensure SensorService is registered.
    /// Handlers are registered as Transient to properly resolve scoped SensorService dependencies.
    /// MQTT settings are configured from the "Sensors:Mqtt" configuration section.
    /// </remarks>
    public static WebApplicationBuilder AddMqttInfrastructure(this WebApplicationBuilder builder)
    {
        // Register MqttSettings from Sensors domain configuration using Options pattern
        builder.Services.Configure<MqttSettings>(options =>
        {
            var sensorsSettings = builder.Configuration
                .GetSection("Sensors")
                .Get<SensorsSettings>() ?? new SensorsSettings();

            options.BrokerHost = sensorsSettings.Mqtt.BrokerHost;
            options.BrokerPort = sensorsSettings.Mqtt.BrokerPort;
            options.ClientId = sensorsSettings.Mqtt.ClientId;
            options.Username = sensorsSettings.Mqtt.Username;
            options.Password = sensorsSettings.Mqtt.Password;
            options.TopicConveyor = sensorsSettings.Mqtt.Topics.Conveyor;
            options.TopicBaking = sensorsSettings.Mqtt.Topics.Baking;
            options.TopicDough = sensorsSettings.Mqtt.Topics.Dough;
            options.TopicPacking = sensorsSettings.Mqtt.Topics.Packing;
        });

        // Add MQTT internals
        builder.Services.AddSingleton<MqttMessageRouter>();
        builder.Services.AddHostedService<MqttListenerService>();
        builder.Services.AddSingleton<MqttWorkQueue>();
        builder.Services.AddHostedService<MqttProcessingService>();

        // Add handlers as Transient to allow scoped dependency injection
        // Each time a handler is requested, a new instance is created with properly scoped dependencies
        builder.Services.AddTransient<IMqttMessageHandler, ConveyorHandler>();
        builder.Services.AddTransient<IMqttMessageHandler, BakingHandler>();
        builder.Services.AddTransient<IMqttMessageHandler, DoughHandler>();
        builder.Services.AddTransient<IMqttMessageHandler, PackingHandler>();

        return builder;
    }
}
