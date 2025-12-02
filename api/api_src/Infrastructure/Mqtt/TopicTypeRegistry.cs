namespace Infrastructure.Mqtt;

public interface ITopicTypeRegistry
{
   bool TryGetMessageType(string topic, out Type? type);
}

public class TopicTypeRegistry : ITopicTypeRegistry
{
    private readonly Dictionary<string, Type> _map;

    public TopicTypeRegistry(Dictionary<string, Type> map)
    {
        _map = map;
    }

    public bool TryGetMessageType(string topic, out Type? type) => _map.TryGetValue(topic, out type);
}