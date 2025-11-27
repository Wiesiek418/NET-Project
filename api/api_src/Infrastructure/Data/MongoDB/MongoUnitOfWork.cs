using MongoDB.Driver;
using Core.Abstractions;

namespace Infrastructure.Data.MongoDB;

/// <summary>
/// Base unit of work for MongoDB contexts.
/// Manages multiple repositories within a single domain context.
/// </summary>
public abstract class MongoUnitOfWork : IUnitOfWork
{
    protected readonly IMongoDatabase _database;
    private readonly Dictionary<Type, object> _repositories = new();

    protected MongoUnitOfWork(IMongoDatabase database)
    {
        _database = database;
    }

    public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = CreateRepository<TEntity>();
        }
        return (IRepository<TEntity>)_repositories[type];
    }

    /// <summary>
    /// Override to create domain-specific repositories.
    /// </summary>
    protected abstract object CreateRepository<TEntity>() where TEntity : class;

    public virtual Task SaveChangesAsync(CancellationToken ct = default)
    {
        // MongoDB writes are atomic at the document level by default.
        // If you need transactional behavior across documents, implement it here.
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _repositories.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        _repositories.Clear();
        await Task.CompletedTask;
    }
}
