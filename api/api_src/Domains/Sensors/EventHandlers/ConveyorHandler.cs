using System.Text.Json;
using Microsoft.Extensions.Options;
using Domains.Sensors.Models;
using Domains.Sensors.Application;
using Infrastructure.Mqtt;

namespace Domains.Sensors.EventHandlers;

public class ConveyorHandler : IMqttMessageHandler
{
    private readonly ILogger<ConveyorHandler> _logger;
    private readonly SensorService _sensorService;

    public string TopicFilter { get; }

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

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling conveyor message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<ConveyorBeltReading>(payload) 
            ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");
        return _sensorService.SaveConveyorReadingAsync(reading, ct);
    }
}
