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

public class ConveyorHandler : IMqttMessageHandler
{
    private readonly ILogger<ConveyorHandler> _logger;
    private readonly IConveyorService _service;

    public string TopicFilter { get; }

    public ConveyorHandler(
        ILogger<ConveyorHandler> logger,
        IConveyorService service,
        IOptions<MqttSettings> options
        )
    {
        _logger = logger;
        _service = service;
        TopicFilter = options.Value.TopicConveyor;
    }

    public Task HandleMessageAsync(string payload, CancellationToken ct)
    {
        _logger.LogDebug($"Handling message from {TopicFilter}");

        var reading = JsonSerializer.Deserialize<ConveyorBeltReading>(payload) ?? throw new ArgumentNullException("Message deserialized to null");
        return _service.CreateAsync(reading);
    }
}
