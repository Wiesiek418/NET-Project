using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

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


public class PackingLineReading : BaseReading
{
    [BsonElement("conveyorSpeed")]
    public double ConveyorSpeed { get; set; }       // m/s

    [BsonElement("packageCount")]
    public int PackageCount { get; set; }           // liczba zapakowanych porcji

    [BsonElement("sealTemperature")]
    public double SealTemperature { get; set; }     // °C

    [BsonElement("errorCount")]
    public int ErrorCount { get; set; }             // błędne opakowania

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty; // "running" | "stopped" | "maintenance" | "error"
}
