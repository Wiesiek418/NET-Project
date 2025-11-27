namespace Infrastructure.Mqtt;

public class MqttSettings
{
    public string BrokerHost { get; set; } = string.Empty;
    public int BrokerPort { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TopicConveyor { get; set; } = string.Empty;
    public string TopicBaking { get; set; } = string.Empty;
    public string TopicDough { get; set; } = string.Empty;
    public string TopicPacking { get; set; } = string.Empty;
}