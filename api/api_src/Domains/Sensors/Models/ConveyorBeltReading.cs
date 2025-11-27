using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Sensors.Models;

public static class ConveyorBeltStatus
{
    public const string Active = "active";
    public const string Stopped = "stopped";
    public const string Misaligned = "misaligned";
    public const string Ripped = "ripped";

    public static readonly HashSet<string> All =
        new() { Active, Stopped, Misaligned, Ripped };
}

public class ConveyorBeltReading : SensorReading
{
    [BsonElement("status")] public string Status { get; set; } = string.Empty;

    [BsonElement("bearingTemp")] public double BearingTemp { get; set; }

    [BsonElement("speed")] public double Speed { get; set; }
}