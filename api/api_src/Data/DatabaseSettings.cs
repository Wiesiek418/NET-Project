namespace Data;

public class MongoSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
    public string ConveyorCollection { get; set; } = null!;
    public string BakingCollection { get; set; } = "bakingReadings";
    public string DoughCollection { get; set; } = "doughReadings";
    public string PackingCollection { get; set; } = "packingReadings";
}