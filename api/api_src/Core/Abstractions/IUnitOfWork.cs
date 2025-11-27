namespace Core.Abstractions;

/// <summary>
///     Unit of Work pattern - manages multiple repositories for a domain context.
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    /// <summary>
    ///     Saves changes across all repositories.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}