using System.Text.RegularExpressions;

namespace Infrastructure.Mqtt;

/// <summary>
/// Routes MQTT messages to appropriate handlers based on topic matching.
/// Lazily resolves handlers from DI scope to support scoped dependencies.
/// </summary>
public class MqttMessageRouter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MqttMessageRouter> _logger;
    private IEnumerable<IMqttMessageHandler>? _handlersCache;

    public MqttMessageRouter(
        IServiceProvider serviceProvider,
        ILogger<MqttMessageRouter> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RouteMessageAsync(string topic, string payload, CancellationToken ct)
    {
        // Resolve handlers from DI container in current scope
        // This allows handlers with scoped dependencies to work correctly
        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IMqttMessageHandler>();
        
        var matchedHandlers = handlers.Where(h => TopicMatches(h.TopicFilter, topic)).ToList();

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
