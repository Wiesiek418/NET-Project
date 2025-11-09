using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class DoughRepository : MongoRepository<DoughMixerReading>, IDoughRepository
{
    public DoughRepository(MongoDbContext context)
        : base(context.DoughReadings) { }
}
