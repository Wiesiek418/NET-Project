using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(AlphaReading))]
public abstract class BaseReading
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    //public DateTime Timestamp { get; set; }
}