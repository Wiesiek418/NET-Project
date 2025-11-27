namespace Infrastructure.Mqtt;

public class MqttProcessingService : BackgroundService
{
    private readonly ILogger<MqttProcessingService> _logger;
    private readonly MqttWorkQueue _queue;
    private readonly MqttMessageRouter _router;

    public MqttProcessingService(
        MqttWorkQueue queue,
        MqttMessageRouter router,
        ILogger<MqttProcessingService> logger)
    {
        _queue = queue;
        _router = router;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var (topic, payload) in
                       _queue.Queue.Reader.ReadAllAsync(stoppingToken))
            try
            {
                await _router.RouteMessageAsync(topic, payload, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MQTT message");
            }
    }
}