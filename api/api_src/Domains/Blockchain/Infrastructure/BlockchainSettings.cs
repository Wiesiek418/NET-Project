namespace Domains.Blockchain.Infrastructure;

/// <summary>
///     Settings for the Blockchain domain.
///     Contains blockchain configuration and MongoDB collection names.
///     Database connection is shared at application level.
/// </summary>
public class BlockchainSettings
{
    public string RpcUrl { get; set; } = "https://sepolia.infura.io/v3/3a9054388e6748308aac294ff334b4c2";
    public string TokenAddress { get; set; } = "0x76a7854fB1f3bAd944F7aD88D5b9f749430f5613";
    public int TokenDecimals { get; set; } = 18;
    public string AppWalletPrivateKey { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; } = 0.001m;
    public BlockchainCollectionsSettings Wallets { get; set; } = new();
}

/// <summary>
///     MongoDB collection names for blockchain data.
/// </summary>
public class BlockchainCollectionsSettings
{
    public string Wallets { get; set; } = "wallets";
}