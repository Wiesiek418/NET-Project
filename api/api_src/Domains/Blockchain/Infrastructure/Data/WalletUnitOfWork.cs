using Domains.Blockchain.Models;
using Infrastructure.Data.MongoDB;

namespace Domains.Blockchain.Infrastructure.Data;

/// <summary>
///     Unit of Work for Wallet domain.
///     Manages wallet-related repositories.
/// </summary>
public class WalletUnitOfWork : MongoUnitOfWork
{
    private readonly WalletMongoContext _walletContext;

    public WalletUnitOfWork(WalletMongoContext walletContext)
        : base(walletContext.Database)
    {
        _walletContext = walletContext;
    }

    protected override object CreateRepository<TEntity>()
    {
        if (typeof(TEntity) == typeof(WalletInfo))
            return new MongoRepository<WalletInfo>(_walletContext.GetCollection<WalletInfo>());

        throw new NotSupportedException($"Entity type '{typeof(TEntity).Name}' is not supported by WalletUnitOfWork");
    }
}