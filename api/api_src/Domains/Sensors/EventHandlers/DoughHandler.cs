using System.Text.Json;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;
using Microsoft.Extensions.Options;

namespace Domains.Sensors.EventHandlers;

public class DoughHandler : IMqttMessageHandler
{
    private readonly ILogger<DoughHandler> _logger;
    private readonly SensorService _sensorService;

    public DoughHandler(
        ILogger<DoughHandler> logger,
        SensorService sensorService,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _sensorService = sensorService;
        TopicFilter = options.Value.TopicDough;
    }

    public string TopicFilter { get; }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling dough message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<DoughMixerReading>(payload)
                      ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");

        return _sensorService.SaveDoughReadingAsync(reading, ct);
    }
}