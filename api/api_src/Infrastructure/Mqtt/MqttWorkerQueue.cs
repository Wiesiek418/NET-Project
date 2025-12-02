using System.Threading.Channels;

namespace Infrastructure.Mqtt;

public class MqttQueueMessage
{
    public string Topic { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
}

public class MqttWorkQueue
{
    private readonly Channel<MqttQueueMessage> _channel;

    public MqttWorkQueue(int capacity = 1000)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateBounded<MqttQueueMessage>(options);
    }

    // Enqueue single message
    public async Task EnqueueAsync(MqttQueueMessage message, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(message, ct);
    }

    // Dequeue single message
    public async Task<MqttQueueMessage> DequeueAsync(CancellationToken ct = default)
    {
        return await _channel.Reader.ReadAsync(ct);
    }

    // Batch dequeue up to maxItems, waits if no messages available
    public async Task<List<MqttQueueMessage>> DequeueBatchAsync(int maxItems = 10, CancellationToken ct = default)
    {
        var batch = new List<MqttQueueMessage>(maxItems);

        // Always wait for at least one message
        var first = await _channel.Reader.ReadAsync(ct);
        batch.Add(first);

        // Try to read remaining messages without waiting
        while (batch.Count < maxItems && _channel.Reader.TryRead(out var msg))
        {
            batch.Add(msg);
        }

        return batch;
    }
}
