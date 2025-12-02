using Domains.Sensors.EventHandlers;
using Domains.Sensors.Infrastructure.Data;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;

namespace Domains.Sensors.Infrastructure;

public static class AddSensorsInfrastructureSetup
{
    public static WebApplicationBuilder AddSensorsInfrastructure(this WebApplicationBuilder builder)
    {
        // Register domain settings
        builder.Services.Configure<SensorsSettings>(
            builder.Configuration.GetSection("Sensors"));

        // Register infrastructure
        builder.Services.AddScoped<SensorsMongoContext>();
        builder.Services.AddScoped(sp =>
            new SensorsUnitOfWork(sp.GetRequiredService<SensorsMongoContext>()));

        // Register application services
        builder.Services.AddScoped<SensorService>();

        // Register MQTT handlers
        builder.AddSensorsMqttHandlers();

        return builder;
    }

    public static WebApplicationBuilder AddSensorsMqttHandlers(this WebApplicationBuilder builder)
    {
        var sensorsSettings = builder.Configuration
            .GetSection("Sensors")
            .Get<SensorsSettings>() ?? new SensorsSettings();

        builder.Services.AddSingleton<ITopicTypeRegistry>(sp =>
            new TopicTypeRegistry(
                new Dictionary<string, Type>
                {
                    [sensorsSettings.Mqtt.Topics.Conveyor] = typeof(ConveyorBeltReading),
                    [sensorsSettings.Mqtt.Topics.Baking] = typeof(BakingFurnaceReading),
                    [sensorsSettings.Mqtt.Topics.Dough] = typeof(DoughMixerReading),
                    [sensorsSettings.Mqtt.Topics.Packing] = typeof(PackingLineReading),
                }
            ));


        // Register generic sensor handler for all sensor reading types
        builder.Services.AddTransient(typeof(IMqttMessageHandler<>), typeof(SensorHandler<>));

        return builder;
    }
}