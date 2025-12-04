using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domains.Sensors.API;

[ApiController]
[Route("api/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly SensorService _sensorService;

    public SensorsController(SensorService sensorService)
    {
        _sensorService = sensorService;
    }

    // GET /api/sensors - Summary of all sensors
    [HttpGet]
    public async Task<IEnumerable<SensorSummary>> GetAll(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetAllSensorsSummaryAsync(filter, sort, ct);
    }

    // GET /api/sensors/conveyor - All conveyor readings
    [HttpGet("conveyor")]
    [HttpGet("ConveyorBeltReading")]
    public async Task<IEnumerable<ConveyorBeltReading>> GetConveyor(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetReadingsAsync<ConveyorBeltReading>(filter, sort, ct);
    }

    // GET /api/sensors/baking - All baking readings
    [HttpGet("baking")]
    [HttpGet("BakingFurnaceReading")]
    public async Task<IEnumerable<BakingFurnaceReading>> GetBaking(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetReadingsAsync<BakingFurnaceReading>(filter, sort, ct);
    }

    // GET /api/sensors/dough - All dough readings
    [HttpGet("dough")]
    [HttpGet("DoughMixerReading")]
    public async Task<IEnumerable<DoughMixerReading>> GetDough(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetReadingsAsync<DoughMixerReading>(filter, sort, ct);
    }

    // GET /api/sensors/packing - All packing readings
    [HttpGet("packing")]
    [HttpGet("PackingLineReading")]
    public async Task<IEnumerable<PackingLineReading>> GetPacking(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetReadingsAsync<PackingLineReading>(filter, sort, ct);
    }
}