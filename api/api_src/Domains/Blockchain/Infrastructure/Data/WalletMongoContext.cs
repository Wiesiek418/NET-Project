using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Infrastructure.Data.MongoDB;
using Core.Abstractions;
using Domains.Blockchain.Infrastructure;

namespace Domains.Blockchain.Infrastructure.Data;

/// <summary>
/// MongoDB context for Blockchain domain.
/// Manages blockchain wallet and transaction data.
/// Uses shared MongoDB database connection.
/// </summary>
public class WalletMongoContext
{
    private readonly IMongoDatabase _database;
    private readonly IOptions<BlockchainSettings> _settings;

    public WalletMongoContext(
        IMongoDatabase database,
        IOptions<BlockchainSettings> settings)
    {
        _database = database;
        _settings = settings;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName = null!) where T : class
    {
        var name = collectionName ?? typeof(T).Name.ToLowerInvariant();
        return _database.GetCollection<T>(name);
    }

    public IMongoDatabase Database => _database;
}
