using Domains.Blockchain.Application;
using Domains.Sensors.Infrastructure.Data;
using Domains.Sensors.Models;
using Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Domains.Sensors.Application;

/// <summary>
///     Application service for sensor domain operations.
///     Uses repositories abstracted from MongoDB.
///     Supports filtering and sorting of sensor readings.
/// </summary>
public class SensorService
{
    private readonly SensorsMongoContext _context;
    private readonly SensorsUnitOfWork _unitOfWork;
    private readonly WalletService _walletService;

    public SensorService(
        SensorsUnitOfWork unitOfWork,
        SensorsMongoContext context,
        WalletService walletService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _walletService = walletService;
    }

    public async Task<IEnumerable<SensorType>> GetReadingsAsync<SensorType>(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
        where SensorType : SensorReading
    {
        var repository = _unitOfWork.GetRepository<SensorType>();
        return await repository.GetAllAsync(filter, sort, ct);
    }

    public async Task<SensorType?> GetReadingByIdAsync<SensorType>(string id, CancellationToken ct = default) where SensorType : SensorReading
    {
        var repository = _unitOfWork.GetRepository<SensorType>();
        return await repository.GetByIdAsync(id, ct);
    }

    public async Task SaveReadingAsync<SensorType>(SensorType reading, CancellationToken ct = default) where SensorType : SensorReading
    {
        var repository = _unitOfWork.GetRepository<SensorType>();
        await repository.CreateAsync(reading, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        
        await _walletService.RegisterOrUpdateSensorAsync(reading.SensorId, typeof(SensorType).Name, reading.WalletAddress, ct);
    }

    #region Global Sensor Operations

    public async Task<IEnumerable<SensorSummary>> GetAllSensorsSummaryAsync(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
    {
        var conveyorTask = GetCollectionSummaryAsync(_context.ConveyorReadings, "Conveyor", ct);
        var bakingTask = GetCollectionSummaryAsync(_context.BakingReadings, "Baking", ct);
        var doughTask = GetCollectionSummaryAsync(_context.DoughReadings, "Dough", ct);
        var packingTask = GetCollectionSummaryAsync(_context.PackingReadings, "Packing", ct);

        await Task.WhenAll(conveyorTask, bakingTask, doughTask, packingTask);

        var allSensors = conveyorTask.Result
            .Concat(bakingTask.Result)
            .Concat(doughTask.Result)
            .Concat(packingTask.Result);

        return allSensors.ApplyQuery(filter, sort);
    }

    // Pomocnicza metoda generyczna do grupowania (Group By) w MongoDB
    private async Task<List<SensorSummary>> GetCollectionSummaryAsync<T>(
        IMongoCollection<T> collection,
        string typeName,
        CancellationToken ct) where T : SensorReading
    {
        var query = collection.AsQueryable()
            .GroupBy(r => r.SensorId)
            .Select(g => new SensorSummary
            {
                SensorId = g.Key,
                Type = typeName,
                CreatedAt = g.Min(r => r.Timestamp)
            });

        return await query.ToListAsync(ct);
    }

    #endregion
}