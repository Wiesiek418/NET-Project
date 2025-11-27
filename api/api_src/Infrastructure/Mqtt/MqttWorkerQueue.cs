using System.Threading.Channels;

namespace Infrastructure.Mqtt;

public class MqttWorkQueue
{
    public Channel<(string Topic, string Payload)> Queue { get; } 
        = Channel.CreateUnbounded<(string, string)>();
}
