using Microsoft.AspNetCore.Mvc;
using Domains.Sensors.Models;
using Domains.Sensors.Application;

namespace Domains.Sensors.API;

[ApiController]
[Route("api/[controller]")]
public class BakingController : ControllerBase
{
    private readonly SensorService _sensorService;

    public BakingController(SensorService sensorService) =>
        _sensorService = sensorService;

    [HttpGet]
    public async Task<IEnumerable<BakingFurnaceReading>> Get(
        [FromQuery] string? filter, 
        [FromQuery] string? sort,
        CancellationToken ct) =>
        await _sensorService.GetAllBakingReadingsAsync(filter, sort, ct);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BakingFurnaceReading reading, CancellationToken ct)
    {
        if (reading == null) return BadRequest();
        await _sensorService.SaveBakingReadingAsync(reading, ct);
        return CreatedAtAction(nameof(Get), null);
    }
}
