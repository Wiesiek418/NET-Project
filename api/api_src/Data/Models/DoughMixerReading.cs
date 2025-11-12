using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

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


public class DoughMixerReading : BaseReading
{
    [BsonElement("rotationSpeed")]
    public double RotationSpeed { get; set; }       // obr/min

    [BsonElement("motorTemperature")]
    public double MotorTemperature { get; set; }    // °C

    [BsonElement("vibrationLevel")]
    public double VibrationLevel { get; set; }      // g

    [BsonElement("loadWeight")]
    public double LoadWeight { get; set; }          // kg

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;  // "mixing" | "idle" | "error"
}
