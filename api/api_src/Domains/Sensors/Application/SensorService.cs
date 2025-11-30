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

    #region Conveyor Belt Readings

    public async Task<IEnumerable<ConveyorBeltReading>> GetAllConveyorReadingsAsync(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<ConveyorBeltReading>();
        return await repository.GetAllAsync(filter, sort, ct);
    }

    public async Task<ConveyorBeltReading?> GetConveyorReadingByIdAsync(string id, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<ConveyorBeltReading>();
        return await repository.GetByIdAsync(id, ct);
    }

    public async Task SaveConveyorReadingAsync(ConveyorBeltReading reading, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<ConveyorBeltReading>();
        await repository.CreateAsync(reading, ct);

        await _walletService.RegisterOrUpdateSensorAsync(reading.SensorId, "Conveyor", reading.WalletAddress, ct);
        if (!string.IsNullOrEmpty(reading.WalletAddress))
        {
            await _walletService.SendTokenAsync(reading.WalletAddress, ct);
        }
        
        await _unitOfWork.SaveChangesAsync(ct);
    }

    #endregion

    #region Baking Furnace Readings

    public async Task<IEnumerable<BakingFurnaceReading>> GetAllBakingReadingsAsync(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<BakingFurnaceReading>();
        return await repository.GetAllAsync(filter, sort, ct);
    }

    public async Task<BakingFurnaceReading?> GetBakingReadingByIdAsync(string id, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<BakingFurnaceReading>();
        return await repository.GetByIdAsync(id, ct);
    }

    public async Task SaveBakingReadingAsync(BakingFurnaceReading reading, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<BakingFurnaceReading>();
        await repository.CreateAsync(reading, ct);

        await _walletService.RegisterOrUpdateSensorAsync(reading.SensorId, "Baking", reading.WalletAddress, ct);
        if (!string.IsNullOrEmpty(reading.WalletAddress))
        {
            await _walletService.SendTokenAsync(reading.WalletAddress, ct);
        }
        
        await _unitOfWork.SaveChangesAsync(ct);
    }

    #endregion

    #region Dough Mixer Readings

    public async Task<IEnumerable<DoughMixerReading>> GetAllDoughReadingsAsync(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<DoughMixerReading>();
        return await repository.GetAllAsync(filter, sort, ct);
    }

    public async Task<DoughMixerReading?> GetDoughReadingByIdAsync(string id, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<DoughMixerReading>();
        return await repository.GetByIdAsync(id, ct);
    }

    public async Task SaveDoughReadingAsync(DoughMixerReading reading, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<DoughMixerReading>();
        await repository.CreateAsync(reading, ct);

        await _walletService.RegisterOrUpdateSensorAsync(reading.SensorId, "Dough", reading.WalletAddress, ct);
        if (!string.IsNullOrEmpty(reading.WalletAddress))
        {
            await _walletService.SendTokenAsync(reading.WalletAddress, ct);
        }
        
        await _unitOfWork.SaveChangesAsync(ct);
    }

    #endregion

    #region Packing Line Readings

    public async Task<IEnumerable<PackingLineReading>> GetAllPackingReadingsAsync(
        string? filter = null,
        string? sort = null,
        CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<PackingLineReading>();
        return await repository.GetAllAsync(filter, sort, ct);
    }

    public async Task<PackingLineReading?> GetPackingReadingByIdAsync(string id, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<PackingLineReading>();
        return await repository.GetByIdAsync(id, ct);
    }

    public async Task SavePackingReadingAsync(PackingLineReading reading, CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<PackingLineReading>();
        await repository.CreateAsync(reading, ct);

        await _walletService.RegisterOrUpdateSensorAsync(reading.SensorId, "Packing", reading.WalletAddress, ct);
        if (!string.IsNullOrEmpty(reading.WalletAddress))
        {
            await _walletService.SendTokenAsync(reading.WalletAddress, ct);
        }
        
        await _unitOfWork.SaveChangesAsync(ct);
    }

    #endregion

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