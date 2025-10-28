using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Options;

using Data.Services;
using Data.Services.Interfaces;
using Queue.Mqtt;
using Data.Models;
using System.Buffers.Text;
using System.Text;

namespace Queue.Mqtt.Handlers;

public class AlphaHandler : IMqttMessageHandler
{
    private readonly ILogger<AlphaHandler> _logger;
    private readonly IAlphaService _service;

    public string TopicFilter { get; }

    public AlphaHandler(
        ILogger<AlphaHandler> logger,
        IAlphaService service,
        IOptions<MqttSettings> options
        )
    {
        _logger = logger;
        _service = service;
        TopicFilter = options.Value.TopicAlpha;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<AlphaReading>(payload) ?? throw new ArgumentNullException("Message deserialized to null");
        return _service.CreateAsync(reading);
    }
}