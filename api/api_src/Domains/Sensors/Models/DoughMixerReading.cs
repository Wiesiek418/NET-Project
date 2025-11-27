using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Sensors.Models;

public static class DoughMixerStatus
{
    public const string Mixing = "mixing";
    public const string Idle = "idle";
    public const string Error = "error";

    public static readonly HashSet<string> All = new()
    {
        Mixing, Idle, Error
    };
}

public class DoughMixerReading : SensorReading
{
    [BsonElement("rotationSpeed")] public double RotationSpeed { get; set; }

    [BsonElement("motorTemperature")] public double MotorTemperature { get; set; }

    [BsonElement("vibrationLevel")] public double VibrationLevel { get; set; }

    [BsonElement("loadWeight")] public double LoadWeight { get; set; }

    [BsonElement("status")] public string Status { get; set; } = string.Empty;
}