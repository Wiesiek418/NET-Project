using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Core.Entities;

namespace Domains.Sensors.Models;

/// <summary>
/// Base class for all sensor readings.
/// </summary>
[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(
    typeof(ConveyorBeltReading),
    typeof(BakingFurnaceReading),
    typeof(DoughMixerReading),
    typeof(PackingLineReading)
)]
public abstract class SensorReading : Entity
{
    [BsonElement("sensorId")]
    public int SensorId { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
}
