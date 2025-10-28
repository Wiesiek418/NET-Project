namespace Queue.Mqtt;

public class MqttSettings
{
    public string BrokerHost { get; set; } = string.Empty;
    public int BrokerPort { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TopicAlpha { get; set; } = string.Empty;
}