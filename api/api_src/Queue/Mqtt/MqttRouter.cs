using System.Text.RegularExpressions;
using System.Buffers;

using Queue.Mqtt.Handlers;

namespace Queue.Mqtt;

public class MqttMessageRouter
{
    private readonly IEnumerable<IMqttMessageHandler> _handlers;
    private readonly ILogger<MqttMessageRouter> _logger;

    public MqttMessageRouter(
        IEnumerable<IMqttMessageHandler> handlers,
        ILogger<MqttMessageRouter> logger)
    {
        _handlers = handlers;
        _logger = logger;
    }

    public async Task RouteMessageAsync(string topic, string payload, CancellationToken ct)
    {
        var matchedHandlers = _handlers.Where(h => TopicMatches(h.TopicFilter, topic)).ToList();

        if (!matchedHandlers.Any())
        {
            _logger.LogWarning("No handler found for topic: {Topic}", topic);
            return;
        }

        foreach (var handler in matchedHandlers)
        {
            try
            {
                await handler.HandleMessageAsync(payload, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling topic {Topic} by {Handler}", topic, handler.GetType().Name);
            }
        }
    }

    private static bool TopicMatches(string filter, string topic)
    {
        // Simplified MQTT wildcard match: + for one level, # for many
        var pattern = "^" + Regex.Escape(filter)
            .Replace("\\+", "[^/]+")
            .Replace("\\#", ".+") + "$";
        return Regex.IsMatch(topic, pattern);
    }
}
