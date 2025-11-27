using Core.Abstractions;
using Domains.Sensors.Infrastructure.Data;
using Domains.Sensors.Models;

namespace Domains.Sensors.Application;

/// <summary>
/// Application service for sensor domain operations.
/// Uses repositories abstracted from MongoDB.
/// Supports filtering and sorting of sensor readings.
/// </summary>
public class SensorService
{
    private readonly SensorsUnitOfWork _unitOfWork;

    public SensorService(SensorsUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveChangesAsync(ct);
    }

    #endregion
}
