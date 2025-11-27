using System.Text.Json;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;
using Microsoft.Extensions.Options;

namespace Domains.Sensors.EventHandlers;

public class PackingHandler : IMqttMessageHandler
{
    private readonly ILogger<PackingHandler> _logger;
    private readonly SensorService _sensorService;

    public PackingHandler(
        ILogger<PackingHandler> logger,
        SensorService sensorService,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _sensorService = sensorService;
        TopicFilter = options.Value.TopicPacking;
    }

    public string TopicFilter { get; }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling packing message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<PackingLineReading>(payload)
                      ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");

        return _sensorService.SavePackingReadingAsync(reading, ct);
    }
}