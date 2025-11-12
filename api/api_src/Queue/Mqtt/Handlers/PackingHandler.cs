using System.Text.Json;
using Microsoft.Extensions.Options;
using Data.Services.Interfaces;
using Data.Models;
using Queue.Mqtt;

namespace Queue.Mqtt.Handlers;

public class PackingHandler : IMqttMessageHandler
{
    private readonly ILogger<PackingHandler> _logger;
    private readonly IPackingService _service;
    public string TopicFilter { get; }

    public PackingHandler(
        ILogger<PackingHandler> logger,
        IPackingService service,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _service = service;
        TopicFilter = options.Value.TopicPacking;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling packing message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<PackingLineReading>(payload)
            ?? throw new ArgumentNullException("Message deserialized to null");

        return _service.CreateAsync(reading);
    }
}
