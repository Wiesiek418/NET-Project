using MongoDB.Driver;

using Data.Models;

namespace Data.Repository;

public class ConveyorRepository : MongoRepository<ConveyorBeltReading>, IConveyorRepository
{
    public ConveyorRepository(MongoDbContext context)
        : base(context.ConveyorReadings) {}
}
