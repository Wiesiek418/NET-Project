namespace Domains.Sensors.Infrastructure;

/// <summary>
/// Settings for the Sensors domain.
/// Contains MongoDB collection names and MQTT configuration.
/// Database connection is shared at application level.
/// </summary>
public class SensorsSettings
{
    public SensorsCollectionsSettings Collections { get; set; } = new();
    public SensorsMqttSettings Mqtt { get; set; } = new();
}

/// <summary>
/// MongoDB collection names for different sensor types.
/// </summary>
public class SensorsCollectionsSettings
{
    public string Conveyor { get; set; } = "conveyor_readings";
    public string Baking { get; set; } = "baking_readings";
    public string Dough { get; set; } = "dough_readings";
    public string Packing { get; set; } = "packing_readings";
}

/// <summary>
/// MQTT configuration for the Sensors domain.
/// </summary>
public class SensorsMqttSettings
{
    public string BrokerHost { get; set; } = "localhost";
    public int BrokerPort { get; set; } = 1883;
    public string ClientId { get; set; } = "mqtt-listener";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public SensorsMqttTopicsSettings Topics { get; set; } = new();
}

/// <summary>
/// MQTT topic configuration for sensor types.
/// </summary>
public class SensorsMqttTopicsSettings
{
    public string Conveyor { get; set; } = "sensors/conveyor";
    public string Baking { get; set; } = "sensors/baking";
    public string Dough { get; set; } = "sensors/dough";
    public string Packing { get; set; } = "sensors/packing";
}
