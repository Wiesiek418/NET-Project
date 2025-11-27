namespace Infrastructure.Data.MongoDB;

/// <summary>
///     Shared database settings for all domains.
///     All domains use the same MongoDB connection and database.
/// </summary>
public class MongoDBSettings
{
    public string ConnectionString { get; set; } = "mongodb://root:example@localhost:27017";
    public string DatabaseName { get; set; } = "sensordb";
}