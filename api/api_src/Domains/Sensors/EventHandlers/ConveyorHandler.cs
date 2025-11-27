using System.Text.Json;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;
using Microsoft.Extensions.Options;

namespace Domains.Sensors.EventHandlers;

public class ConveyorHandler : IMqttMessageHandler
{
    private readonly ILogger<ConveyorHandler> _logger;
    private readonly SensorService _sensorService;

    public ConveyorHandler(
        ILogger<ConveyorHandler> logger,
        SensorService sensorService,
        IOptions<MqttSettings> options
    )
    {
        _logger = logger;
        _sensorService = sensorService;
        TopicFilter = options.Value.TopicConveyor;
    }

    public string TopicFilter { get; }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling conveyor message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<ConveyorBeltReading>(payload)
                      ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");
        return _sensorService.SaveConveyorReadingAsync(reading, ct);
    }
}