using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Infrastructure.Data.MongoDB;
using Core.Abstractions;
using Domains.Sensors.Models;
using Domains.Sensors.Infrastructure;

namespace Domains.Sensors.Infrastructure.Data;

/// <summary>
/// MongoDB context for Sensor domain.
/// Exposes collections for different sensor types (Conveyor, Baking, Dough, Packing).
/// Uses shared MongoDB database connection.
/// </summary>
public class SensorsMongoContext
{
    private readonly IMongoDatabase _database;
    private readonly IOptions<SensorsSettings> _settings;

    public SensorsMongoContext(
        IMongoDatabase database,
        IOptions<SensorsSettings> settings)
    {
        _database = database;
        _settings = settings;
    }

    public IMongoCollection<ConveyorBeltReading> ConveyorReadings =>
        _database.GetCollection<ConveyorBeltReading>(_settings.Value.Collections.Conveyor);

    public IMongoCollection<BakingFurnaceReading> BakingReadings =>
        _database.GetCollection<BakingFurnaceReading>(_settings.Value.Collections.Baking);

    public IMongoCollection<DoughMixerReading> DoughReadings =>
        _database.GetCollection<DoughMixerReading>(_settings.Value.Collections.Dough);

    public IMongoCollection<PackingLineReading> PackingReadings =>
        _database.GetCollection<PackingLineReading>(_settings.Value.Collections.Packing);

    public IMongoDatabase Database => _database;
}
