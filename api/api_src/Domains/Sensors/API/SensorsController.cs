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

    // GET /api/sensors?filter=type:eq:Conveyor&sort=sensorId:asc
    [HttpGet]
    public async Task<IEnumerable<SensorSummary>> GetAll(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct)
    {
        return await _sensorService.GetAllSensorsSummaryAsync(filter, sort, ct);
    }
}