using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Data.Models;

public static class BakingFurnaceStatus
{
    public const string Baking = "baking";
    public const string Heating = "heating";
    public const string Cooling = "cooling";
    public const string Error = "error";

    public static readonly HashSet<string> All = new()
    {
        Baking, Heating, Cooling, Error
    };
}

public static class BakingFurnaceDoorStatus
{
    public const string Open = "open";
    public const string Closed = "closed";

    public static readonly HashSet<string> All = new()
    {
        Open, Closed
    };
}


public class BakingFurnaceReading : BaseReading
{
    [BsonElement("temperature")]
    public double Temperature { get; set; }   // °C

    [BsonElement("humidity")]
    public double Humidity { get; set; }      // %

    [BsonElement("gasFlow")]
    public double GasFlow { get; set; }       // m³/h

    [BsonElement("doorStatus")]
    public string DoorStatus { get; set; } = string.Empty; // "closed" | "open"

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;     // "baking" | "heating" | "cooling" | "error"
}
