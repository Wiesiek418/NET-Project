using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Core.Entities;

namespace Domains.Blockchain.Models;

/// <summary>
/// Wallet entity - represents a blockchain wallet.
/// </summary>
public class WalletInfo : Entity
{
    [BsonElement("type")]
    public string SensorType { get; set; } = string.Empty;

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
}
