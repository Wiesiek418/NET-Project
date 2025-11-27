using Microsoft.AspNetCore.Mvc;
using Domains.Sensors.Models;
using Domains.Sensors.Application;

namespace Domains.Sensors.API;

[ApiController]
[Route("api/[controller]")]
public class DoughController : ControllerBase
{
    private readonly SensorService _sensorService;

    public DoughController(SensorService sensorService) =>
        _sensorService = sensorService;

    [HttpGet]
    public async Task<IEnumerable<DoughMixerReading>> Get(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct) =>
        await _sensorService.GetAllDoughReadingsAsync(filter, sort, ct);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DoughMixerReading reading, CancellationToken ct)
    {
        if (reading == null) return BadRequest();
        await _sensorService.SaveDoughReadingAsync(reading, ct);
        return CreatedAtAction(nameof(Get), null);
    }
}
