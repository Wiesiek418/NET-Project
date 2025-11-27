using System.Text.Json;
using Microsoft.Extensions.Options;
using Domains.Sensors.Models;
using Domains.Sensors.Application;
using Infrastructure.Mqtt;

namespace Domains.Sensors.EventHandlers;

public class DoughHandler : IMqttMessageHandler
{
    private readonly ILogger<DoughHandler> _logger;
    private readonly SensorService _sensorService;
    public string TopicFilter { get; }

    public DoughHandler(
        ILogger<DoughHandler> logger,
        SensorService sensorService,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _sensorService = sensorService;
        TopicFilter = options.Value.TopicDough;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling dough message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<DoughMixerReading>(payload)
            ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");

        return _sensorService.SaveDoughReadingAsync(reading, ct);
    }
}
