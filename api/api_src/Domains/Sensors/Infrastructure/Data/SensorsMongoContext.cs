using Domains.Sensors.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Domains.Sensors.Infrastructure.Data;

/// <summary>
///     MongoDB context for Sensor domain.
///     Exposes collections for different sensor types (Conveyor, Baking, Dough, Packing).
///     Uses shared MongoDB database connection.
/// </summary>
public class SensorsMongoContext
{
    private readonly IOptions<SensorsSettings> _settings;

    public SensorsMongoContext(
        IMongoDatabase database,
        IOptions<SensorsSettings> settings)
    {
        Database = database;
        _settings = settings;
    }

    public IMongoCollection<ConveyorBeltReading> ConveyorReadings =>
        Database.GetCollection<ConveyorBeltReading>(_settings.Value.Collections.Conveyor);

    public IMongoCollection<BakingFurnaceReading> BakingReadings =>
        Database.GetCollection<BakingFurnaceReading>(_settings.Value.Collections.Baking);

    public IMongoCollection<DoughMixerReading> DoughReadings =>
        Database.GetCollection<DoughMixerReading>(_settings.Value.Collections.Dough);

    public IMongoCollection<PackingLineReading> PackingReadings =>
        Database.GetCollection<PackingLineReading>(_settings.Value.Collections.Packing);

    public IMongoDatabase Database { get; }
}