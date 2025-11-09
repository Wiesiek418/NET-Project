using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Data.Models;

namespace Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoSettings _settings;

    public MongoDbContext(IOptions<MongoSettings> settings)
    {
        _settings = settings.Value;

        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoDatabase Database => _database;

    public IMongoCollection<ConveyorBeltReading> ConveyorReadings =>
        _database.GetCollection<ConveyorBeltReading>(_settings.ConveyorCollection);

    public IMongoCollection<BakingFurnaceReading> BakingReadings =>
        _database.GetCollection<BakingFurnaceReading>(_settings.BakingCollection);

    public IMongoCollection<DoughMixerReading> DoughReadings =>
        _database.GetCollection<DoughMixerReading>(_settings.DoughCollection);

    public IMongoCollection<PackingLineReading> PackingReadings =>
        _database.GetCollection<PackingLineReading>(_settings.PackingCollection);
}