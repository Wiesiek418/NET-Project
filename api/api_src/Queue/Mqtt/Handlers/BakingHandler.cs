using System.Text.Json;
using Microsoft.Extensions.Options;
using Data.Services.Interfaces;
using Data.Models;
using Queue.Mqtt;

namespace Queue.Mqtt.Handlers;

public class BakingHandler : IMqttMessageHandler
{
    private readonly ILogger<BakingHandler> _logger;
    private readonly IBakingService _service;
    public string TopicFilter { get; }

    public BakingHandler(
        ILogger<BakingHandler> logger,
        IBakingService service,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _service = service;
        TopicFilter = options.Value.TopicBaking;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling baking message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<BakingFurnaceReading>(payload)
            ?? throw new ArgumentNullException("Message deserialized to null");

        return _service.CreateAsync(reading);
    }
}
