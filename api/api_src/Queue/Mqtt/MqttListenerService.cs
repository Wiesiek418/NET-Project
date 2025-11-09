using MQTTnet;
using Microsoft.Extensions.Options;
using System.Text;

namespace Queue.Mqtt;

public class MqttListenerService : BackgroundService
{
    private readonly ILogger<MqttListenerService> _logger;
    private readonly MqttSettings _settings;
    private readonly MqttMessageRouter _router;
    private IMqttClient? _mqttClient;

    public MqttListenerService(
        ILogger<MqttListenerService> logger,
        IOptions<MqttSettings> options,
        MqttMessageRouter router
        )
    {
        _logger = logger;
        _router = router;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mqttFactory = new MqttClientFactory();
        _mqttClient = mqttFactory.CreateMqttClient();

        if (_mqttClient == null)
        {
            throw new Exception("MQTTClient failed to start");
        }

        var options = new MqttClientOptionsBuilder()
            .WithClientId(_settings.ClientId)
            .WithTcpServer(_settings.BrokerHost, _settings.BrokerPort)
            .WithCleanSession()
            .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                string msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                await _router.RouteMessageAsync(
                    e.ApplicationMessage.Topic,
                    msg,
                    stoppingToken);
            } 
            catch (DecoderFallbackException ex)
            {
                _logger.LogWarning($"Failed to decode message from {e.ApplicationMessage.Topic} - {ex}");
            }
        };

        await _mqttClient.ConnectAsync(options, stoppingToken);

        var subscribeBuilder = mqttFactory.CreateSubscribeOptionsBuilder();
        // add topic filters only when configured
        if (!string.IsNullOrWhiteSpace(_settings.TopicConveyor))
            subscribeBuilder = subscribeBuilder.WithTopicFilter(_settings.TopicConveyor);
        if (!string.IsNullOrWhiteSpace(_settings.TopicBaking))
            subscribeBuilder = subscribeBuilder.WithTopicFilter(_settings.TopicBaking);
        if (!string.IsNullOrWhiteSpace(_settings.TopicDough))
            subscribeBuilder = subscribeBuilder.WithTopicFilter(_settings.TopicDough);
        if (!string.IsNullOrWhiteSpace(_settings.TopicPacking))
            subscribeBuilder = subscribeBuilder.WithTopicFilter(_settings.TopicPacking);

        var mqttSubscribeOptions = subscribeBuilder.Build();
        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, stoppingToken);
    }

    #region Disconnect

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await StopClientAsync();
        await base.StopAsync(cancellationToken);
    }

    private async Task StopClientAsync()
    {
        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            try
            {
                await _mqttClient.DisconnectAsync();
                _logger.LogInformation("MQTT client disconnected cleanly.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while disconnecting MQTT client.");
            }
        }

        _mqttClient?.Dispose();
        _mqttClient = null;
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing MqttListenerService...");
        _mqttClient?.Dispose();
        base.Dispose();
    }

    #endregion
}
