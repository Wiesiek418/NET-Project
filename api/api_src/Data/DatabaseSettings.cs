namespace Data;

public class MongoSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
    public string AlphaCollection { get; set; } = null!;
}