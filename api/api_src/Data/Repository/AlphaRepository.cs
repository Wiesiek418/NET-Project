using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class AlphaRepository : MongoRepository<AlphaReading>, IAlphaRepository
{
    public AlphaRepository(MongoDbContext context)
        : base(context.AlphaReadings) {}

    public async Task<IEnumerable<AlphaReading>> GetAllByValueAsync(int value) =>
        await _collection.Find(x => x.Value == value).ToListAsync();
}
