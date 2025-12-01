using Domains.Sensors.EventHandlers;
using Domains.Sensors.Infrastructure;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;

namespace Infrastructure.Mqtt;

/// <summary>
/// Extension methods for configuring MQTT infrastructure with the new clean architecture.
/// Registers shared MQTT services and delegates to domain-specific handlers registration.
/// Handlers are registered as Transient to allow proper scoped dependency injection.
/// Settings are registered using the Options pattern with configuration binding.
/// </summary>
public static class MqttInfrastructureSetpu
{
    public static WebApplicationBuilder AddMqttInfrastructure(this WebApplicationBuilder builder)
    {
        // Register MqttSettings from Sensors domain configuration using Options pattern
        var sensorsSettings = builder.Configuration
            .GetSection("Sensors")
            .Get<SensorsSettings>() ?? new SensorsSettings();

        builder.Services.Configure<MqttSettings>(options =>
        {
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

        // Add core MQTT services
        builder.Services.AddHostedService<MqttListenerService>();
        builder.Services.AddSingleton<MqttWorkQueue>();
        builder.Services.AddHostedService<MqttProcessingService>();
        builder.Services.AddScoped<MqttDispatcher>();

        return builder;
    }
}