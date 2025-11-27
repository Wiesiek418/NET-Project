using Microsoft.AspNetCore.Mvc;
using Domains.Sensors.Models;
using Domains.Sensors.Application;

namespace Domains.Sensors.API;

[ApiController]
[Route("api/[controller]")]
public class PackingController : ControllerBase
{
    private readonly SensorService _sensorService;

    public PackingController(SensorService sensorService) =>
        _sensorService = sensorService;

    [HttpGet]
    public async Task<IEnumerable<PackingLineReading>> Get(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct) =>
        await _sensorService.GetAllPackingReadingsAsync(filter, sort, ct);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PackingLineReading reading, CancellationToken ct)
    {
        if (reading == null) return BadRequest();
        await _sensorService.SavePackingReadingAsync(reading, ct);
        return CreatedAtAction(nameof(Get), null);
    }
}
