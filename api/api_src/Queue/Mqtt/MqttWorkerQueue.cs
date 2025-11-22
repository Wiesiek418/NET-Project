using System.Threading.Channels;

namespace Queue.Mqtt;

public class MqttWorkQueue
{
    public Channel<(string Topic, string Payload)> Queue { get; } 
        = Channel.CreateUnbounded<(string, string)>();
}
