using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Domains.Blockchain.Infrastructure.Data;

/// <summary>
///     MongoDB context for Blockchain domain.
///     Manages blockchain wallet and transaction data.
///     Uses shared MongoDB database connection.
/// </summary>
public class WalletMongoContext
{
    private readonly IOptions<BlockchainSettings> _settings;

    public WalletMongoContext(
        IMongoDatabase database,
        IOptions<BlockchainSettings> settings)
    {
        Database = database;
        _settings = settings;
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<T> GetCollection<T>(string collectionName = null!) where T : class
    {
        var name = collectionName ?? typeof(T).Name.ToLowerInvariant();
        return Database.GetCollection<T>(name);
    }
}