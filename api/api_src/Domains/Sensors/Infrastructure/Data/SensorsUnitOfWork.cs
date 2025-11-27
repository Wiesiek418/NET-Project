using MongoDB.Driver;
using Infrastructure.Data.MongoDB;
using Core.Abstractions;
using Domains.Sensors.Models;

namespace Domains.Sensors.Infrastructure.Data;

/// <summary>
/// Unit of Work for Sensors domain.
/// Manages all sensor reading repositories (Conveyor, Baking, Dough, Packing).
/// </summary>
public class SensorsUnitOfWork : MongoUnitOfWork
{
    private readonly SensorsMongoContext _sensorsContext;

    public SensorsUnitOfWork(SensorsMongoContext sensorsContext) 
        : base(sensorsContext.Database)
    {
        _sensorsContext = sensorsContext;
    }

    protected override object CreateRepository<TEntity>()
    {
        if (typeof(TEntity) == typeof(ConveyorBeltReading))
            return new MongoRepository<ConveyorBeltReading>(_sensorsContext.ConveyorReadings);

        if (typeof(TEntity) == typeof(BakingFurnaceReading))
            return new MongoRepository<BakingFurnaceReading>(_sensorsContext.BakingReadings);

        if (typeof(TEntity) == typeof(DoughMixerReading))
            return new MongoRepository<DoughMixerReading>(_sensorsContext.DoughReadings);

        if (typeof(TEntity) == typeof(PackingLineReading))
            return new MongoRepository<PackingLineReading>(_sensorsContext.PackingReadings);

        throw new NotSupportedException($"Entity type '{typeof(TEntity).Name}' is not supported by SensorsUnitOfWork");
    }
}
