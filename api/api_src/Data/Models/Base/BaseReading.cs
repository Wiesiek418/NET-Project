using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(
    typeof(ConveyorBeltReading),
    typeof(BakingFurnaceReading),
    typeof(DoughMixerReading),
    typeof(PackingLineReading)
)]
public abstract class BaseReading
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("sensorId")]
    public int SensorId { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
}