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

    public IMongoCollection<AlphaReading> AlphaReadings =>
        _database.GetCollection<AlphaReading>(_settings.AlphaCollection);
}