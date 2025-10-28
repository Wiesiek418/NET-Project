using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

public class AlphaReading : BaseReading
{
    public int Value { get; set; }
}