using Core.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Blockchain.Models;

/// <summary>
///     Wallet entity - represents a blockchain wallet.
/// </summary>
public class WalletInfo : Entity
{
    [BsonElement("sensorId")] public int SensorId { get; set; }

    [BsonElement("type")] public string SensorType { get; set; } = string.Empty;

    [BsonElement("address")] public string Address { get; set; } = string.Empty;
}