using Core.Entities;

namespace Domains.Blockchain.Models;

public class WalletBalance : Entity
{
    public int SensorId { get; set; }
    public string SensorType { get; set; } = string.Empty;
    public double Balance { get; set; }
}