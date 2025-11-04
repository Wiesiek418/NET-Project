using System.Buffers;

namespace Queue.Mqtt.Handlers;

public interface IMqttMessageHandler
{
    string TopicFilter { get; } // could be a full topic or wildcard pattern
    Task HandleMessageAsync(string payload, CancellationToken ct);
}