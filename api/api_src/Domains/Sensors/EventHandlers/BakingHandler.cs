using System.Text.Json;
using Microsoft.Extensions.Options;
using Domains.Sensors.Models;
using Domains.Sensors.Application;
using Infrastructure.Mqtt;

namespace Domains.Sensors.EventHandlers;

public class BakingHandler : IMqttMessageHandler
{
    private readonly ILogger<BakingHandler> _logger;
    private readonly SensorService _sensorService;
    public string TopicFilter { get; }

    public BakingHandler(
        ILogger<BakingHandler> logger,
        SensorService sensorService,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _sensorService = sensorService;
        TopicFilter = options.Value.TopicBaking;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling baking message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<BakingFurnaceReading>(payload)
            ?? throw new ArgumentNullException(nameof(payload), "Message deserialized to null");

        return _sensorService.SaveBakingReadingAsync(reading, ct);
    }
}
