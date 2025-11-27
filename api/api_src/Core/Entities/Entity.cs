using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;

/// <summary>
/// Base class for all entities with an Id.
/// </summary>
public abstract class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
