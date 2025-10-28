using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class MongoRepository<T> : IRepository<T> where T : BaseReading
{
    protected readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<T?> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(T entity) =>
        await _collection.InsertOneAsync(entity);
}
