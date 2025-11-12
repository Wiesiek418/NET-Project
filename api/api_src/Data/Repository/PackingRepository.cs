using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class PackingRepository : MongoRepository<PackingLineReading>, IPackingRepository
{
    public PackingRepository(MongoDbContext context)
        : base(context.PackingReadings) { }
}
