using System.Text.Json;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Mqtt;

public class MqttDispatcher
{
    private readonly ITopicTypeRegistry _registry;
    private readonly IServiceProvider _sp;
    private readonly JsonSerializerOptions _json;
    private readonly ILogger<MqttDispatcher> _logger;
    private readonly IHubContext<NotificationHub> _hub;

    public MqttDispatcher(
        ITopicTypeRegistry registry, 
        IServiceProvider sp,
        ILogger<MqttDispatcher> logger,
        IHubContext<NotificationHub> hub)
    {
        _registry = registry;
        _sp = sp;
        _logger = logger;
        _hub = hub;
        _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task DispatchAsync(string topic, string payload, CancellationToken ct)
    {
        // 1. get actual CLR type for topic
        var exists = _registry.TryGetMessageType(topic, out var messageType);
        if (!exists || messageType == null)
        {
            _logger.LogWarning($"No message type registered for topic '{topic}'.");
            return;
        }

        // 2. deserialize strongly typed message
        var message = JsonSerializer.Deserialize(payload, messageType, _json);

        if (message is null)
        {
            throw new InvalidOperationException($"Failed to deserialize message for topic '{topic}'.");
        }

        // 3. resolve the handler for this *concrete* type
        var handlerType = typeof(IMqttMessageHandler<>).MakeGenericType(messageType);
        dynamic handler = _sp.GetRequiredService(handlerType);

        // 4. invoke strongly typed handler
        await handler.HandleAsync((dynamic)message, ct);

        // 5. broadcast to WebSocket clients
        await _hub.Clients.All.SendAsync("NewReading", payload, ct);
    }
}
