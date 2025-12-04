namespace Infrastructure.Mqtt;

public class MqttProcessingService : BackgroundService
{
    private readonly ILogger<MqttProcessingService> _logger;
    private readonly MqttWorkQueue _queue;
    private readonly IServiceProvider _sp;
    private readonly int _batchSize = 10;

    public MqttProcessingService(
        MqttWorkQueue queue,
        IServiceProvider sp,
        ILogger<MqttProcessingService> logger,
        int batchSize = 10)
    {
        _queue = queue;
        _sp = sp;
        _logger = logger;
        _batchSize = batchSize;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var batch = await _queue.DequeueBatchAsync(_batchSize, stoppingToken);

            // Process messages in parallel, each with its own scope
            var tasks = batch.Select(async message =>
            {
                using var scope = _sp.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<MqttDispatcher>();

                try
                {
                    await dispatcher.DispatchAsync(message.Topic, message.Payload, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing MQTT message on topic {Topic}", message.Topic);
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}