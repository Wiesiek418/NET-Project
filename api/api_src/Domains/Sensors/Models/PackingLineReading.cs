using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Sensors.Models;

public static class PackingLineStatus
{
    public const string Running = "running";
    public const string Stopped = "stopped";
    public const string Maintenance = "maintenance";
    public const string Error = "error";

    public static readonly HashSet<string> All = new()
    {
        Running, Stopped, Maintenance, Error
    };
}

public class PackingLineReading : SensorReading
{
    [BsonElement("conveyorSpeed")] public double ConveyorSpeed { get; set; }

    [BsonElement("packageCount")] public int PackageCount { get; set; }

    [BsonElement("sealTemperature")] public double SealTemperature { get; set; }

    [BsonElement("errorCount")] public int ErrorCount { get; set; }

    [BsonElement("status")] public string Status { get; set; } = string.Empty;
}