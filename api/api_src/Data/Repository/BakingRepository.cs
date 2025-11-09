using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class BakingRepository : MongoRepository<BakingFurnaceReading>, IBakingRepository
{
    public BakingRepository(MongoDbContext context)
        : base(context.BakingReadings) { }
}
