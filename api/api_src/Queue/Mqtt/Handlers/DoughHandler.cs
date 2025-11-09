using System.Text.Json;
using Microsoft.Extensions.Options;
using Data.Services.Interfaces;
using Data.Models;
using Queue.Mqtt;

namespace Queue.Mqtt.Handlers;

public class DoughHandler : IMqttMessageHandler
{
    private readonly ILogger<DoughHandler> _logger;
    private readonly IDoughService _service;
    public string TopicFilter { get; }

    public DoughHandler(
        ILogger<DoughHandler> logger,
        IDoughService service,
        IOptions<MqttSettings> options)
    {
        _logger = logger;
        _service = service;
        TopicFilter = options.Value.TopicDough;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling dough message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<DoughMixerReading>(payload)
            ?? throw new ArgumentNullException("Message deserialized to null");

        return _service.CreateAsync(reading);
    }
}
