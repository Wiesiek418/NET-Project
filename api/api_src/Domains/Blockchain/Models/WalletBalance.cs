using Core.Entities;

namespace Domains.Blockchain.Models;

public class WalletBalance : Entity
{
    public string SensorType { get; set; } = string.Empty;
    public double Balance { get; set; }
}